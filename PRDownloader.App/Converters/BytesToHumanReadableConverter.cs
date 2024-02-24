using System.Globalization;

namespace PRDownloader.App.Converters;

public class BytesToHumanReadableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is not long bytes) { throw new Exception(); }
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        if (bytes == 0)
        {
            return $"0 {suffixes[0]}/s";
        }

        int magnitude = (int)Math.Floor(Math.Log(bytes, 1024));
        double readable = bytes / Math.Pow(1024, magnitude);
        string suffix = suffixes[magnitude];

        return $"{readable:N1} {suffix}/s";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new Exception();
    }
}
