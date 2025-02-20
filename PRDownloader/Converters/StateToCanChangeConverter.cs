using System;
using System.Globalization;
using System.Windows.Data;
using MonoTorrent.Client;

namespace PRDownloader.Converters;

[ValueConversion(typeof(TorrentState), typeof(bool))]
public sealed class StateToCanChangeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TorrentState state) { throw new Exception(); }
        return state switch
        {
            TorrentState.Stopped => true,
            TorrentState.Paused => true,
            TorrentState.Starting => false,
            TorrentState.Downloading => true,
            TorrentState.Seeding => true,
            TorrentState.Hashing => false,
            TorrentState.HashingPaused => false,
            TorrentState.Stopping => false,
            TorrentState.Error => false,
            TorrentState.Metadata => false,
            TorrentState.FetchingHashes => false,
            _ => throw new NotImplementedException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new Exception();
    }
}
