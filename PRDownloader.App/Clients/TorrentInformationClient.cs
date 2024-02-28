using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PRDownloader.App.Models;
using PRDownloader.App.Options;
using System.Text.Json;

namespace PRDownloader.App.Clients;

public sealed class TorrentInformationClient
{
    private readonly HttpClient _client;
    private readonly Uri _VersionUri;
    private readonly ILogger<TorrentInformationClient> _logger;

    public TorrentInformationClient(HttpClient client, IOptions<TorrentOptions> options, ILogger<TorrentInformationClient> logger)
    {
        _client = client;
        _VersionUri = options.Value.VersionUrl;
        _logger = logger;
    }

    public async Task<Result<TorrentInformation>> GetInformationAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(_VersionUri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail("Unable to fetch information");
        }

        try
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var information = JsonSerializer.Deserialize<TorrentInformation>(content);
            if (information is not null)
            {
                return Result.Ok(information);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to parse response.");
        }
        return Result.Fail("Failed to parse response");
    }
}
