using PRDownloader.App.Viewmodels;

namespace PRDownloader.App;

public partial class DownloadLocationPage : ContentPage
{
    public DownloadLocationPage(DownloadLocationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
