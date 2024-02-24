namespace PRDownloader.App.Options;

public sealed record TorrentOptions
{
    public required Uri VersionUrl { get; init; }
    public required string SaveDirectory { get; init; }
}
