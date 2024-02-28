namespace PRDownloader.App.Pages.Download;

public partial class DownloadPage : ContentPage
{
    public DownloadPage(DownloadPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
