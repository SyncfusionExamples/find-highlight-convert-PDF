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
                var outputPath = await Task.Run(() => ConvertAndCompress(inputPath, findText, compression));

                StatusMessage = $"PDF created: {outputPath}";

                // Open / share the result
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(outputPath)
                });
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

        private bool CanConvert() => !IsBusy && !string.IsNullOrWhiteSpace(SelectedFilePath);

        partial void OnSelectedFilePathChanged(string? value) => ConvertCommand.NotifyCanExecuteChanged();
        partial void OnIsBusyChanged(bool value) => ConvertCommand.NotifyCanExecuteChanged();

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
