using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Storage;
using PRDownloader.App.Services;

namespace PRDownloader.App.Pages.Settings;

public partial class SettingsPageViewModel : ObservableObject
{
    private readonly IFolderPicker _folderPicker;
    private readonly IShellNavigationService _navigation;
    private readonly IPreferences _preferences;
    [ObservableProperty]
    private string _downloadLocation = string.Empty;

    public SettingsPageViewModel(IShellNavigationService navigation, IFolderPicker folderPicker, IPreferences preferences, TorrentInformationProvider infoProvider)
    {
        _preferences = preferences;
        DownloadLocation = _preferences.Get(PreferenceKey.DownloadLocation, GetDefaultDownloadFolder());
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
            _preferences.Set(PreferenceKey.DownloadLocation, DownloadLocation);
        }
    }

    [RelayCommand]
    private Task ContinueToNextPage()
    {
        return _navigation.GoToAsync("///MainPage");
    }

    private static string GetDefaultDownloadFolder()
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        return Path.Combine(userFolder, "Downloads");
    }
}
