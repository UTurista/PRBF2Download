using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using PRDownloader.Entities;

namespace PRDownloader.Clients;

public sealed class TorrentInformationClient
{
    private readonly HttpClient _client;
    private readonly Uri _VersionUri;

    public TorrentInformationClient(HttpClient client)
    {
        _client = client;
        _VersionUri = new Uri("https://d76a05d74f889aafd38d-39162a6e09ffdab7394e3243fa2342c1.ssl.cf2.rackcdn.com/version.json");
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
           // _logger.LogError(ex, "Unable to parse response.");
        }
        return Result.Fail("Failed to parse response");
    }
}
