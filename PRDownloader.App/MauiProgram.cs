using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PRDownloader.App.Clients;
using PRDownloader.App.Options;
using PRDownloader.App.Pages;
using PRDownloader.App.Services;
using PRDownloader.App.Viewmodels;
using System.Reflection;

namespace PRDownloader.App;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Configuration.AddConfiguration(CreateConfiguration());
       
        builder.Services.AddLogging();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<DownloadLocationPage>();
        builder.Services.AddTransient<DownloadLocationViewModel>();
        builder.Services.AddTransient<PRDownloader>();
        builder.Services.AddSingleton<EngineSettingsProvider>();
        builder.Services.AddHttpClient<TorrentInformationClient>();
        builder.Services.AddSingleton(FolderPicker.Default);
        builder.Services.AddSingleton<TorrentInformationProvider>();
        builder.Services.AddSingleton<IShellNavigationService, ShellNavigationService>();

        builder.Services.Configure<TorrentOptions>(builder.Configuration.GetSection("Torrent"));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static IConfiguration CreateConfiguration()
    {
        var builder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("PRDownloader.App.appsettings.json");
        if (stream is not null)
        {
            builder.AddJsonStream(stream);
        }
        
        return builder.Build();
    }
}
