using System.Text.Json.Serialization;

namespace PRDownloader.Entities;
public sealed record TorrentInformation
{
    [JsonPropertyName("version_big")]
    public string? VersionBig { get; init; }

    [JsonPropertyName("version_small")]
    public string? VersionSmall { get; init; }

    [JsonPropertyName("torrent_url")]
    public string? TorrentUrl { get; init; }

    [JsonPropertyName("torrent_magnet")]
    public string? TorrentMagnet { get; init; }

    [JsonPropertyName("torrent_setupname")]
    public string? TorrentSetupName { get; init; }

    [JsonPropertyName("torrent_foldername")]
    public string? TorrentFolderName { get; init; }
}
