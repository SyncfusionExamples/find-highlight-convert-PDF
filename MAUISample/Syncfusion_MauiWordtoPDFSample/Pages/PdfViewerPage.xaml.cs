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
            string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "ModifiedDocument.pdf");
            using FileStream fileStream = File.Create(fileName);

            // Save the PDF document to the stream.
            PDFViewer.SaveDocument(fileStream);

            await DisplayAlertAsync("Save Document", $"Successfully saved \n Location: {fileName}", "Ok");
        }
    }
}
