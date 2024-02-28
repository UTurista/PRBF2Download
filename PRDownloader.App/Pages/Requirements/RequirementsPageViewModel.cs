using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRDownloader.App.Services;

namespace PRDownloader.App.Pages.Requirements;

public partial class RequirementsPageViewModel : ObservableObject
{
    private readonly TorrentInformationProvider _provider;
    private IShellNavigationService _navigationService;
    private readonly IPreferences _preferences;
    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public RequirementsPageViewModel(TorrentInformationProvider provider, IShellNavigationService navigationService, IPreferences preferences)
    {
        _provider = provider;
        _navigationService = navigationService;
        _preferences = preferences;
    }

    internal async Task StartRequirementsCheck()
    {
        var result = await _provider.GetInformationAsync();
        if (result.IsSuccess)
        {
            var downloadLocation = _preferences.Get(PreferenceKey.DownloadLocation, string.Empty);
            var nextPage = Directory.Exists(downloadLocation) ? "///DownloadPage" : "///SettingsPage";
            await _navigationService.GoToAsync(nextPage);
        }
        else
        {
            ErrorMessage = "Unable to access";
        }
    }
}
