using Syncfusion.Maui.PdfViewer;
using Syncfusion_MauiWordtoPDFSample.PageModels;

namespace Syncfusion_MauiWordtoPDFSample.Pages
{
    public partial class PdfViewerPage : ContentPage
    {
        public PdfViewerPage(PdfViewerPageModel model)
        {
            InitializeComponent();
            AddSaveButton();
            BindingContext = model;
        }

        private void AddSaveButton()
        {
            Button fileSaveButton = new Button
            {
                Text = "\ue75f",
                FontSize = 24,
                FontFamily = "MauiMaterialAssets",
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Transparent,
                TextColor=Colors.Black,
                Padding = 10,
                Style = base.Style,
            };
            fileSaveButton.Clicked += FileSaveButton_Clicked;
#if !WINDOWS && !MACCATALYST
            PDFViewer.Toolbars?.GetByName("TopToolbar")?.Items?.Insert(0, new Syncfusion.Maui.PdfViewer.ToolbarItem(fileSaveButton, "FileSaveButton"));
#else
            PDFViewer?.Toolbars?.GetByName("PrimaryToolbar")?.Items?.Insert(0, new Syncfusion.Maui.PdfViewer.ToolbarItem(fileSaveButton, "FileSaveButton"));
#endif

        }

        private async void FileSaveButton_Clicked(object? sender, EventArgs e)
        {
            string fileName = $"ModifiedDocument_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

#if WINDOWS
            // Save into the user's Downloads folder so the file is easy to find
            // and can be opened with the system's default PDF viewer (not the
            // browser / Edge).
            var downloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            downloads = Path.Combine(downloads, "Downloads");
            if (!Directory.Exists(downloads))
            {
                downloads = FileSystem.Current.AppDataDirectory;
            }
            Directory.CreateDirectory(downloads);
            string fullPath = Path.Combine(downloads, fileName);
#else
            string fullPath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);
#endif

            using FileStream fileStream = File.Create(fullPath);

            // Save the PDF document to the stream.
            PDFViewer.SaveDocument(fileStream);

            await DisplayAlertAsync("Save Document", $"Successfully saved \n Location: {fullPath}", "Ok");

#if WINDOWS
            // Open the file with the registered "open" handler for .pdf so the
            // user gets Acrobat / Foxit / SumatraPDF instead of Edge.
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true,
                    Verb = "open"
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch
            {
                // ignore
            }
#endif
        }
    }
}
