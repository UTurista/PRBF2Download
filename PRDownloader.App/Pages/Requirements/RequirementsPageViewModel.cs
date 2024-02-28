using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PRDownloader.App.Services;

namespace PRDownloader.App.Pages.Requirements;

public partial class RequirementsPageViewModel : ObservableObject
{
    private readonly TorrentInformationProvider _provider;
    private IShellNavigationService _navigationService;
    [ObservableProperty]
    private string _errorMessage;

    public RequirementsPageViewModel(TorrentInformationProvider provider, IShellNavigationService navigationService)
    {
        _provider = provider;
        _navigationService = navigationService;
    }

    internal async Task StartRequirementsCheck()
    {
        var result = await _provider.GetInformationAsync();
        if (result.IsSuccess)
        {
            await _navigationService.GoToAsync("///SettingsPage");
        }
        else
        {
            _errorMessage = "Unable to access";
        }
    }
}
