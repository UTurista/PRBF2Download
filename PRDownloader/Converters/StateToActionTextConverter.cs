// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Data;
using MonoTorrent.Client;

namespace PRDownloader.Converters;

[ValueConversion(typeof(TorrentState), typeof(string))]
public sealed class StateToActionTextConverter : IValueConverter
{
    private const string Resume = "Resume";
    private const string Stop = "Stop";
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TorrentState state) { throw new Exception(); }
        return state switch
        {
            TorrentState.Stopped => Resume,
            TorrentState.Paused => Resume,
            TorrentState.Starting => Stop,
            TorrentState.Downloading => Stop,
            TorrentState.Seeding => Stop,
            TorrentState.Hashing => Resume,
            TorrentState.HashingPaused => Resume,
            TorrentState.Stopping => Resume,
            TorrentState.Error => Resume,
            TorrentState.Metadata => Resume,
            TorrentState.FetchingHashes => Resume,
            _ => throw new NotImplementedException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new Exception();
    }
}
