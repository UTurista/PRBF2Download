using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Storage;
using PRDownloader.App.Services;

namespace PRDownloader.App.Pages.Settings;

public partial class SettingsPageViewModel : ObservableObject
{
    private readonly IFolderPicker _folderPicker;
    private readonly IShellNavigationService _navigation;
    [ObservableProperty]
    private string _downloadLocation = string.Empty;

    public SettingsPageViewModel(IShellNavigationService navigation, IFolderPicker folderPicker, TorrentInformationProvider infoProvider)
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var downloadFolder = Path.Combine(userFolder, "Downloads");
        DownloadLocation = downloadFolder;
        _folderPicker = folderPicker;
        _navigation = navigation;
    }

    [RelayCommand]
    public async Task PickDownloadLocation()
    {
        var result = await _folderPicker.PickAsync();
        if (result.IsSuccessful)
        {
            DownloadLocation = result.Folder.Path;
        }
    }

    [RelayCommand]
    private Task ContinueToNextPage()
    {
        return _navigation.GoToAsync("///MainPage");
    }
}
