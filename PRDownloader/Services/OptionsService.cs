using System;
using System.IO;
using PRDownloader.Entities;

namespace PRDownloader.Services;

public sealed class OptionsService
{
    public readonly static TorrentOptions DefaultState = new()
    {
        AllowDHT = true,
        AllowPeerExchange = true,
        CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ProjectReality", "PRDownloader"),
        DownloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
        LimitDownloadSpeed = null,
        LimitUploadSpeed = null
    };

    public TorrentOptions State { get; private set; } = DefaultState;
    public event EventHandler<TorrentOptions>? StateChanged;

    public void Update(TorrentOptions newState)
    {
        State = newState;
        StateChanged?.Invoke(this, newState);
    }
}
