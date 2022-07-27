using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EconomyViewer.DAL.Entities;

namespace EconomyViewer.DAL.ViewModel;

public class ItemViewModel : ViewModelBase
{
    private Item _selectedItem;
    private List<Item>? _items;

    public List<Item>? Items {
        get => _items;
        set {
            _items = value;
            OnPropertyChanged(nameof(Headers));
        }
    }
    public List<string> Headers => Items?.Select(i => i.Header).ToList() ?? new List<string>();
    public string SelectedHeader {
        get => _selectedItem?.Header ?? "";
        set {
            if (_selectedItem?.Header == value)
                return;
            if (value != null && Headers.Contains(value))
                SelectedItem = (Item)Items!.First(item => item.Header == value).Clone();
        }
    }

    public Item SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StringFormat));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(Mod));

        }
    }
    public string StringFormat => SelectedItem?.StringFormat ?? "";
    public int Count {
        get => SelectedItem?.Count ?? 1;
        set {
            SelectedItem.Count = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(SelectedItem));
        }
    }
    public int Price => SelectedItem?.Price ?? 0;
    public string? Mod => SelectedItem?.Mod;
}
