using System;
using System.Globalization;
using System.Windows.Data;

using EconomyViewer.Model;

namespace EconomyViewer.Converter;
internal class ItemToStringConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Item item ? item.ToString() : (object)"";
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Item.FromString(value.ToString() ?? "", "");
    }
}
