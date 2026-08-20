using Syncfusion_MauiWordtoPDFSample.PageModels;

namespace Syncfusion_MauiWordtoPDFSample.Pages
{
    public partial class WordToPdfPage : ContentPage
    {
        public WordToPdfPage(WordToPdfPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
