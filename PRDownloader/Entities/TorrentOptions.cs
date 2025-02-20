namespace PRDownloader.Entities;

public sealed record TorrentOptions
{
    public required string DownloadPath { get; init; }
    public required string CachePath { get; init; }
    public required uint? LimitDownloadSpeed { get; init; }
    public required uint? LimitUploadSpeed { get; init; }
    public required bool AllowDHT { get; init; }
    public required bool AllowPeerExchange { get; init; }
}
