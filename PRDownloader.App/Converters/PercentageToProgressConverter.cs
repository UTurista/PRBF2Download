using System.Globalization;

namespace PRDownloader.App.Converters;

public class PercentageToProgressConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is double valueAsDouble)
        {
            return valueAsDouble / 100;
        }

        throw new Exception();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double valueAsDouble)
        {
            return valueAsDouble * 100;
        }

        throw new Exception();
    }
}
