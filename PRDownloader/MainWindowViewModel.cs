// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using MonoTorrent;
using MonoTorrent.Client;
using PRDownloader.Clients;
using PRDownloader.Services;

namespace PRDownloader;

public partial class MainWindowViewModel : ObservableObject, IDisposable
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly ClientEngine _engine;
    private readonly TorrentInformationClient _infoClient;
    private readonly WindowsService _windowsService;
    private readonly OptionsService _optionsService;
    private TorrentManager? _prManager;

    [ObservableProperty]
    private TorrentState _state = TorrentState.Stopped;
    [ObservableProperty]
    private long _downloadSpeed;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LaunchGameCommand))]
    private double _downloadProgress;
    [ObservableProperty]
    private long _downloadRemaining;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ValidMagneticUrl))]
    public string _magneticUrl;


    public MainWindowViewModel(EngineSettings settings, TorrentInformationClient infoClient, WindowsService windowsService, OptionsService optionsService, ILogger<MainWindowViewModel> logger)
    {
        _logger = logger;
        _engine = new ClientEngine(settings);
        _infoClient = infoClient;
        MagneticUrl = "";
        _windowsService = windowsService;
        _optionsService = optionsService;
    }

    private bool CanLaunchGame
    {
        get
        {
            if (DownloadProgress < 100)
            {
                return false;
            }

            return GetExecutableSetup() is not null;
        }
    }

    private string? GetExecutableSetup()
    {
        var downloadPath = _optionsService.State.DownloadPath;
        var setupExecutable = Directory.EnumerateFiles(downloadPath, "setup.exe", new EnumerationOptions
        {
            RecurseSubdirectories = true,
            MatchCasing = MatchCasing.CaseInsensitive,
        });

        return setupExecutable.FirstOrDefault();
    }

    public bool ValidMagneticUrl
    {
        get => MagnetLink.TryParse(MagneticUrl, out var _);
    }

    [RelayCommand(CanExecute = nameof(CanToggleProgress))]
    private async Task ToggleProgressAsync()
    {
        if (_prManager is null) { throw new Exception(); }

        if (_prManager.State == TorrentState.Downloading)
        {
            await _prManager.StopAsync();
        }
        else
        {
            await _prManager.StartAsync();
        }
    }

    private bool CanToggleProgress()
    {
        return _prManager switch
        {
            { Complete: true } => false,
            { State: TorrentState.Downloading or TorrentState.Stopped or TorrentState.Paused } => true,
            _ => false
        };
    }

    public async Task StartAsync()
    {
        _logger.LogTrace("Starting torrent {ValidMagneticUrl} {TorrentManager}", ValidMagneticUrl, _prManager);

        if (!ValidMagneticUrl) { return; }
        if (_prManager is not null) { return; }

        // PR Torrent file
        var torrentSettings = new TorrentSettingsBuilder()
        {
            AllowDht = true,
            AllowInitialSeeding = true,
            AllowPeerExchange = true,
            CreateContainingDirectory = true,
            MaximumConnections = 100,
        }.ToSettings();

        var link = MagnetLink.Parse(MagneticUrl);
        _prManager = await _engine.AddAsync(link, @"C:\Users\Vasco\Downloads\prdownload", torrentSettings);
        _prManager.TorrentStateChanged += OnTorrentStateChanged;
        _engine.StatsUpdate += OnStatsUpdate;

        await _prManager.HashCheckAsync(autoStart: false);
    }

    public Task Stop()
    {
        if (_prManager is null) { return Task.CompletedTask; }

        return _prManager.PauseAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _engine.Dispose();
    }

    [RelayCommand(CanExecute = nameof(CanLaunchGame))]
    private void LaunchGame()
    {
        var setup = GetExecutableSetup();
        if (setup is null)
        {
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = setup,
            UseShellExecute = true,
        });

        Application.Current.Shutdown();
    }

    [RelayCommand]
    private void NavigateToOptions()
    {
        _windowsService.OpenOptionsWindow();
    }

    [RelayCommand]
    private static void OpenManual()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://www.realitymod.com/manual/",
            UseShellExecute = true,
        });
    }

    private void OnStatsUpdate(object? sender, StatsUpdateEventArgs e)
    {
        if (_prManager is null) { throw new Exception("OnStatsUpdate"); }
        UpdateTorrentData();
    }

    private void OnTorrentStateChanged(object? sender, TorrentStateChangedEventArgs e)
    {
        if (_prManager is null) { throw new Exception("OnTorrentStateChanged"); }
        UpdateTorrentData();
    }


    private void UpdateTorrentData()
    {
        if (_prManager is null) { throw new Exception("UpdateTorrentData"); }

        Application.Current.Dispatcher.Invoke(() =>
        {
            DownloadSpeed = _prManager.Monitor.DownloadRate;
            DownloadProgress = _prManager.Progress;
            DownloadRemaining = 13333;
            State = _prManager.State;
        });
    }


    internal async void OnActivated()
    {
        _logger.LogTrace("ViewModel activated {ValidMagneticUrl}", ValidMagneticUrl);

        if (!ValidMagneticUrl)
        {
            var info = await _infoClient.GetInformationAsync();
            _logger.LogDebug("torrent info: {@info}", info);

            if (info.IsSuccess && !string.IsNullOrEmpty(info.Value.TorrentMagnet))
            {
                MagneticUrl = info.Value.TorrentMagnet;
                await StartAsync();
            }
        }
    }



    internal void OnDeactivated()
    {
        // throw new NotImplementedException();
    }
}
