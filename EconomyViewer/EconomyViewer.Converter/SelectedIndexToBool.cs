using System;
using System.Globalization;
using System.Windows.Data;

namespace EconomyViewer.Converter;

public class SelectedIndexToBool : IValueConverter
{
    /// <summary>
    /// Converts Selected Index to Bool determining whether value has any selection
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns>If control doesn't have selected index (it actually -1) return false, otherwise true</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        
        return (int)value != -1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
