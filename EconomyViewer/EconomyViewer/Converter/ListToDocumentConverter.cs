using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;

using EconomyViewer.Model;

namespace EconomyViewer.Converter;

internal class ListToDocumentConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (targetType == null || value == null)
            return null;

        List<Item> items = (List<Item>)value;
        FlowDocument document = new FlowDocument();
        foreach (var item in items)
        {
            document.Blocks.Add(new Paragraph(new Run(item.ToString())));
        }
        return document;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
