using CommunityToolkit.Mvvm.ComponentModel;

namespace Syncfusion_MauiWordtoPDFSample.PageModels
{
    public partial class PdfViewerPageModel : ObservableObject
    {
        [ObservableProperty]
        private string? pdfPath;

        [ObservableProperty]
        private MemoryStream? pdfDocumentSource;

        public PdfViewerPageModel(MemoryStream pdfStream)
        {
            pdfDocumentSource = pdfStream;
        }

        public PdfViewerPageModel(string pdfPath)
        {
            PdfPath = pdfPath;
        }
    }
}
