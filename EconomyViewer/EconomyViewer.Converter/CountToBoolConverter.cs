using System;
using System.Globalization;
using System.Windows.Data;

namespace EconomyViewer.Converter;
public class CountToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return targetType != null ? System.Convert.ToInt32(value) > 0 : (object)false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
