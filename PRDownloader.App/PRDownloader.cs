using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonoTorrent;
using MonoTorrent.Client;
using PRDownloader.App.Clients;
using PRDownloader.App.Options;
using PRDownloader.App.Services;

namespace PRDownloader.App;

public partial class PRDownloader : ObservableObject, IDisposable
{
    private readonly ILogger<PRDownloader> _logger;
    private readonly ClientEngine _engine;
    private readonly TorrentInformationClient _torrentInfoClient;
    private readonly TorrentOptions _options;
    private TorrentManager? _prManager;
    [ObservableProperty]
    private TorrentState _state = TorrentState.Stopped;
    [ObservableProperty]
    private int _totalPeers;
    [ObservableProperty]
    private long _downloadSpeed;
    [ObservableProperty]
    private double _downloadProgress;

    public PRDownloader(EngineSettingsProvider provider, TorrentInformationClient torrentInfoClient, IOptionsSnapshot<TorrentOptions> options, ILogger<PRDownloader> logger)
    {
        _logger = logger;
        _options = options.Value;
        _engine = new ClientEngine(provider.Settings);
        _torrentInfoClient = torrentInfoClient;
    }

    public async Task Start()
    {
        if (_prManager is null)
        {
            var magneticLink = await GetMagnetLink();
            if (string.IsNullOrEmpty(magneticLink))
            {
                return;
            }
            await CreateTorrentManager(magneticLink);
        }

        if (_prManager is not null)
        {
            await _prManager.StartAsync();
        }
    }

    private async Task<string> GetMagnetLink()
    {
        var info = await _torrentInfoClient.GetInformationAsync();
        if (info.IsFailed)
        {
            return string.Empty;
        }

        return info.Value.TorrentMagnet ?? string.Empty;
    }

    private async Task<bool> CreateTorrentManager(string magneticUri)
    {
        if(!MagnetLink.TryParse(magneticUri, out var link))
        {
            _logger.LogError("Unable to parse magnetic link: {uri}", magneticUri);
            return false;
        }

        // PR Torrent file
        TorrentSettings torrentSettings = new TorrentSettingsBuilder()
        {
            AllowDht = true,
            AllowInitialSeeding = true,
            AllowPeerExchange = true,
            CreateContainingDirectory = true,
            MaximumConnections = 100,
        }.ToSettings();


        _prManager = await _engine.AddAsync(link, _options.SaveDirectory, torrentSettings);
        _prManager.TorrentStateChanged += OnTorrentStateChanged;
        _engine.StatsUpdate += OnStatsUpdate;
        return true;
    }

    private void OnStatsUpdate(object? sender, StatsUpdateEventArgs e)
    {
        if(_prManager is null) { return; }

        DownloadSpeed = _prManager.Monitor.DownloadSpeed;
        DownloadProgress = _prManager.Progress;
        TotalPeers = _prManager.Peers.Available;
    }

    private void OnTorrentStateChanged(object? sender, TorrentStateChangedEventArgs e)
    {
        State = e.NewState;
    }

    public Task Stop()
    {
        if (_prManager is null) { return Task.CompletedTask; }

        return _prManager.PauseAsync();
    }

    public void Dispose()
    {
        _engine.Dispose();
    }
}
