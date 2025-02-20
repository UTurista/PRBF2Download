using System.Net.Http;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MonoTorrent.Client;
using PRDownloader.Clients;
using PRDownloader.Services;
using Microsoft.Extensions.Logging;

namespace PRDownloader;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider _serviceProvider;

    public App()
    {
        ServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Logging
        services.AddLogging(ConfigureLogging);

        services.AddSingleton<OptionsService>();
        services.AddSingleton<WindowsService>();

        // Options page
        services.AddTransient<OptionsWindowViewModel>();
        services.AddTransient<OptionsWindow>();
        services.AddSingleton<HttpClient>(x => new HttpClient());
        services.AddSingleton<TorrentInformationClient>();
        services.AddSingleton<EngineSettings>(x => EngineSettingsProvider.CreateDefaultSettings());
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
    }

    private static void ConfigureLogging(ILoggingBuilder builder)
    {
        builder
            .AddFilter("Microsoft", LogLevel.Warning)
            .AddFilter("System", LogLevel.Warning)
            .SetMinimumLevel(LogLevel.Trace)
            .AddDebug();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var service = _serviceProvider.GetRequiredService<WindowsService>();
        service.OpenMainWindow();
    }
}
