using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.Maui.Handlers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Windows.Foundation.Metadata;
using System.Runtime.Versioning;

namespace PRDownloader.App;

public partial class App : Application
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger)
    {
        _logger = logger;
        InitializeComponent();
        ConfigureWindowsSettings();
        MainPage = new AppShell();
    }

    public void ConfigureWindowsSettings()
    {
#if WINDOWS
        WindowHandler.Mapper.AppendToMapping(nameof(IWindow), A);
#endif
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Width = 800;
        window.Height = 400;

        return window;
    }

    [SupportedOSPlatform("Windows")]
    private void A(IWindowHandler handler, IWindow window)
    {
        var nativeWindow = handler.PlatformView;
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
        WindowId WindowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
        AppWindow appWindow = AppWindow.GetFromWindowId(WindowId);
        if(appWindow.Presenter is not OverlappedPresenter presenter)
        {
            _logger.LogError("Unexpected presented, got {type}", appWindow.Presenter.GetType().FullName);
            return;
        }
        presenter.IsResizable = false;
        presenter.IsMaximizable = false;
        presenter.IsMinimizable = true;
    }
}
