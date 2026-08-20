using System.ComponentModel;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Color = Syncfusion.Drawing.Color;

namespace Syncfusion_MauiWordtoPDFSample.PageModels
{
    public partial class WordToPdfPageModel : ObservableObject
    {
        [ObservableProperty]
        private string? selectedFilePath;

        [ObservableProperty]
        private string? selectedFileName;

        [ObservableProperty]
        private string? textToFind;

        [ObservableProperty]
        private string compressionLevel = "Normal";

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string? statusMessage;

        [ObservableProperty]
        private bool isConverted;

        private MemoryStream? _pdfStream;

        public IReadOnlyList<string> CompressionOptions { get; } = new[]
        {
            "None",
            "Low",
            "Normal",
            "High",
            "Maximum"
        };

        [RelayCommand]
        private async Task BrowseAsync()
        {
            try
            {
                var docxFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/rtf", "text/rtf" } },
                    { DevicePlatform.iOS, new[] { "com.microsoft.word.doc", "org.openxmlformats.wordprocessingml.document", "public.rtf" } },
                    { DevicePlatform.MacCatalyst, new[] { "docx", "doc", "rtf" } },
                    { DevicePlatform.WinUI, new[] { ".docx", ".doc", ".rtf" } }
                });

                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a Word document",
                    FileTypes = docxFileType
                });

                if (result is not null)
                {
                    SelectedFilePath = result.FullPath;
                    SelectedFileName = result.FileName;
                    StatusMessage = $"Selected: {result.FileName}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error selecting file: {ex.Message}";
            }
        }

        [RelayCommand(CanExecute = nameof(CanConvert))]
        private async Task ConvertAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedFilePath))
            {
                StatusMessage = "Please select a Word document first.";
                return;
            }

            try
            {
                IsBusy = true;
                StatusMessage = "Converting...";

                var inputPath = SelectedFilePath;
                var findText = TextToFind ?? string.Empty;
                var compression = CompressionLevel;

                // Run on a background thread so UI stays responsive
                _pdfStream = await Task.Run(() => ConvertToMemoryStream(inputPath, findText, compression));

                IsConverted = true;
                StatusMessage = "PDF created successfully. Choose an option below.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Conversion failed: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ViewPdfAsync()
        {
            if (_pdfStream == null)
            {
                StatusMessage = "No PDF to view. Please convert first.";
                return;
            }

            _pdfStream.Position = 0;
            var pdfViewerPage = new Pages.PdfViewerPage(new PdfViewerPageModel(_pdfStream));
            await Application.Current!.MainPage!.Navigation.PushAsync(pdfViewerPage);
        }

        [RelayCommand]
        private async Task SavePdfAsync()
        {
            if (_pdfStream == null)
            {
                StatusMessage = "No PDF to save. Please convert first.";
                return;
            }

            try
            {
                var fileName = $"Document_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                // Save into the user's Downloads folder. On Windows this maps to
                // the "Downloads" folder under the user's profile. On Android it
                // is the public Downloads directory. On iOS / MacCatalyst we fall
                // back to the app's own cache directory because the sandbox does
                // not expose a writable Downloads folder directly.
                string savePath = GetDownloadsFilePath(fileName);

                _pdfStream.Position = 0;
                using (var fileStream = File.Create(savePath))
                {
                    await _pdfStream.CopyToAsync(fileStream);
                }

                StatusMessage = $"PDF saved to: {savePath}";

                // Open the file with the system's default PDF viewer (Acrobat
                // Reader, Foxit, Preview, etc.) instead of forcing the user's
                // default browser. On Windows we use the WinUI Launcher which
                // invokes the registered "open with" app for the .pdf extension.
                try
                {
#if WINDOWS
                    OpenPdfWithDefaultAppWindows(savePath);
#else
                    await Launcher.Default.OpenAsync(new OpenFileRequest
                    {
                        Title = fileName,
                        File = new ReadOnlyFile(savePath)
                    });
#endif
                }
                catch
                {
                    // Ignore launcher errors; the file is still saved.
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Save failed: {ex.Message}";
                await Application.Current!.MainPage!.DisplayAlert("Error", $"Failed to save PDF:\n{ex.Message}", "OK");
            }
        }

        private static string GetDownloadsFilePath(string fileName)
        {
#if WINDOWS
            // Windows: use the user's "Downloads" folder. KnownFolderId 0x374D
            // corresponds to FOLDERID_Downloads. The Environment.SpecialFolder
            // enum is a safe cross-version way to get the same path.
            var downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            downloads = Path.Combine(downloads, "Downloads");
            if (!Directory.Exists(downloads))
            {
                // Fallback if the special folder cannot be resolved.
                downloads = FileSystem.Current.AppDataDirectory;
            }
            Directory.CreateDirectory(downloads);
            return Path.Combine(downloads, fileName);
#elif ANDROID
            var downloads = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;
            if (string.IsNullOrEmpty(downloads))
            {
                downloads = FileSystem.Current.AppDataDirectory;
            }
            Directory.CreateDirectory(downloads);
            return Path.Combine(downloads, fileName);
#elif MACCATALYST || IOS
            // Sandboxed iOS / MacCatalyst apps cannot write to a public Downloads
            // folder without a file picker, so use the app's own Documents folder
            // which the user can still access via the Files app.
            var docs = FileSystem.Current.AppDataDirectory;
            Directory.CreateDirectory(docs);
            return Path.Combine(docs, fileName);
#else
            var fallback = FileSystem.Current.AppDataDirectory;
            Directory.CreateDirectory(fallback);
            return Path.Combine(fallback, fileName);
#endif
        }

#if WINDOWS
        private static void OpenPdfWithDefaultAppWindows(string filePath)
        {
            // UseShellExecute = true makes the OS resolve the registered handler
            // for .pdf (Adobe Reader, Foxit, SumatraPDF, ...) instead of
            // forcing the default browser / Microsoft Edge. The verb "open"
            // explicitly requests the default registered application.
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true,
                Verb = "open"
            };
            System.Diagnostics.Process.Start(psi);
        }
#endif

        private bool CanConvert() => !IsBusy && !string.IsNullOrWhiteSpace(SelectedFilePath);

        partial void OnSelectedFilePathChanged(string? value) => ConvertCommand.NotifyCanExecuteChanged();
        partial void OnIsBusyChanged(bool value) => ConvertCommand.NotifyCanExecuteChanged();

        [RelayCommand]
        private void Clear()
        {
            _pdfStream?.Dispose();
            _pdfStream = null;

            SelectedFilePath = null;
            SelectedFileName = null;
            TextToFind = null;
            CompressionLevel = "Normal";
            IsConverted = false;
            StatusMessage = null;
        }

        private static string ConvertAndCompress(string inputPath, string findText, string compressionLevel)
        {
            // 1. Load the document. Pick the right FormatType based on the extension
            //    so that .doc, .docx and .rtf are all supported.
            using var document = LoadDocument(inputPath);

            // 2. Find and highlight occurrences of the search text
            if (!string.IsNullOrWhiteSpace(findText))
            {
                var selections = document.FindAll(findText, false, false);
                if (selections != null)
                {
                    foreach (TextSelection selection in selections)
                    {
                        var range = selection.GetAsOneRange();
                        range.CharacterFormat.HighlightColor = Color.Yellow;
                        range.CharacterFormat.Bold = true;
                    }
                }
            }

            // 3. Convert Word to PDF
            using var renderer = new DocIORenderer();
            using var pdfDocument = renderer.ConvertToPDF(document);

            // 4. Compress the PDF.
            //    The Compress() extension is only available on PdfLoadedDocument,
            //    so we save the freshly-converted document to a memory stream
            //    and reload it before applying compression.
            var outputPath = Path.ChangeExtension(inputPath, ".pdf");
            using var intermediateStream = new MemoryStream();
            pdfDocument.Save(intermediateStream);
            intermediateStream.Position = 0;

            using var loaded = new PdfLoadedDocument(intermediateStream);
            if (!string.Equals(compressionLevel, "None", StringComparison.OrdinalIgnoreCase))
            {
                var options = new PdfCompressionOptions
                {
                    ImageQuality = compressionLevel switch
                    {
                        "Low" => 90,
                        "High" => 50,
                        "Maximum" => 30,
                        _ => 70 // Normal
                    },
                    OptimizeFont = true,
                    OptimizePageContents = true,
                    RemoveMetadata = false
                };
                loaded.Compress(options);
            }

            // 5. Save the final PDF next to the input file
            using var outputStream = File.Create(outputPath);
            loaded.Save(outputStream);

            return outputPath;
        }

        private static MemoryStream ConvertToMemoryStream(string inputPath, string findText, string compressionLevel)
        {
            // 1. Load the document
            using var document = LoadDocument(inputPath);

            // 2. Find and highlight occurrences of the search text
            if (!string.IsNullOrWhiteSpace(findText))
            {
                var selections = document.FindAll(findText, false, false);
                if (selections != null)
                {
                    foreach (TextSelection selection in selections)
                    {
                        var range = selection.GetAsOneRange();
                        range.CharacterFormat.HighlightColor = Color.Yellow;
                        range.CharacterFormat.Bold = true;
                    }
                }
            }

            // 3. Convert Word to PDF
            using var renderer = new DocIORenderer();
            using var pdfDocument = renderer.ConvertToPDF(document);

            // 4. Compress the PDF
            var intermediateStream = new MemoryStream();
            pdfDocument.Save(intermediateStream);
            intermediateStream.Position = 0;

            using var loaded = new PdfLoadedDocument(intermediateStream);
            if (!string.Equals(compressionLevel, "None", StringComparison.OrdinalIgnoreCase))
            {
                var options = new PdfCompressionOptions
                {
                    ImageQuality = compressionLevel switch
                    {
                        "Low" => 90,
                        "High" => 50,
                        "Maximum" => 30,
                        _ => 70 // Normal
                    },
                    OptimizeFont = true,
                    OptimizePageContents = true,
                    RemoveMetadata = false
                };
                loaded.Compress(options);
            }

            // 5. Save to memory stream
            var outputStream = new MemoryStream();
            loaded.Save(outputStream);
            outputStream.Position = 0;

            return outputStream;
        }

        private static WordDocument LoadDocument(string inputPath)
        {
            // Default to Word (.docx) format if the extension is unknown.
            var format = FormatType.Docx;
            var extension = Path.GetExtension(inputPath);
            if (string.Equals(extension, ".rtf", StringComparison.OrdinalIgnoreCase))
            {
                format = FormatType.Rtf;
            }
            else if (string.Equals(extension, ".doc", StringComparison.OrdinalIgnoreCase))
            {
                format = FormatType.Doc;
            }

            return new WordDocument(inputPath, format);
        }
    }
}
