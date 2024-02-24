using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using CommunityToolkit.Maui.Storage;

namespace PRDownloader.App.Viewmodels;

public partial class DownloadLocationViewModel : ObservableObject
{
    private readonly IFolderPicker _folderPicker;
    [ObservableProperty]
    private string _downloadLocation = string.Empty;

    public DownloadLocationViewModel(IFolderPicker folderPicker)
    {
        var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var downloadFolder = Path.Combine(userFolder, "Downloads");
        DownloadLocation = downloadFolder;
        _folderPicker = folderPicker;
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
}
