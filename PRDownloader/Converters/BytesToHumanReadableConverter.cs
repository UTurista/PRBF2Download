// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace PRDownloader.Converters;

[ValueConversion(typeof(long), typeof(string))]
public sealed class BytesToHumanReadableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not long bytes) { throw new Exception(); }
        string[] suffixes = ["B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
        if (bytes == 0)
        {
            return $"0 {suffixes[0]}";
        }

        var magnitude = (int)Math.Floor(Math.Log(bytes, 1024));
        var readable = bytes / Math.Pow(1024, magnitude);
        var suffix = suffixes[magnitude];

        return $"{readable:N1} {suffix}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new Exception();
    }
}
