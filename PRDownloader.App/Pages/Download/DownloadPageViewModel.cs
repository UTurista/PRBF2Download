using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PRDownloader.App.Pages.Download;

public partial class DownloadPageViewModel : ObservableObject
{
    [ObservableProperty]
    private PRDownloader _downloader;

    public DownloadPageViewModel(PRDownloader downloader)
    {
        _downloader = downloader;
    }

    [RelayCommand]
    public async Task Start()
    {
        await Downloader.Start();
    }
}
