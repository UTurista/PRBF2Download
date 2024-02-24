using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PRDownloader.App.Viewmodels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private PRDownloader _downloader;

    public MainPageViewModel(PRDownloader downloader)
    {
        _downloader = downloader;
    }

    [RelayCommand]
    public async Task Start()
    {
        await Downloader.Start();
    }
}
