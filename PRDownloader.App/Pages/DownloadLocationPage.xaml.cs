using PRDownloader.App.Viewmodels;

namespace PRDownloader.App.Pages;

public partial class DownloadLocationPage : ContentPage
{
    public DownloadLocationPage(DownloadLocationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
