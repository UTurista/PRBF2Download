using FluentResults;
using PRDownloader.App.Clients;
using PRDownloader.App.Models;

namespace PRDownloader.App.Services;

public class TorrentInformationProvider
{
    private readonly IServiceProvider _serviceProvider;
    private TorrentInformation? _torrentInformation;

    public TorrentInformationProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TorrentInformation>> GetInformationAsync(CancellationToken cancellationToken = default)
    {
        if(_torrentInformation is not null)
        {
            return _torrentInformation;
        }

        using var scope = _serviceProvider.CreateAsyncScope();
        var client = scope.ServiceProvider.GetRequiredService<TorrentInformationClient>();
        var result = await client.GetInformationAsync(cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        _torrentInformation = result.Value;
        return _torrentInformation;
    }
}
