using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace PRDownloader.Services;

public sealed class WindowsService
{
    private readonly IServiceProvider _provider;

    public WindowsService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void OpenOptionsWindow()
    {
        OpenModal<OptionsWindow>();
    }

    public void OpenMainWindow()
    {
        OpenWindow<MainWindow>();
    }

    private void OpenWindow<TWindow>() where TWindow : Window
    {
        var window = _provider.GetRequiredService<TWindow>();
        window.Show();
    }

    private void OpenModal<TWindow>() where TWindow : Window
    {
        var window = _provider.GetRequiredService<TWindow>();
        window.ShowDialog();
    }
}
