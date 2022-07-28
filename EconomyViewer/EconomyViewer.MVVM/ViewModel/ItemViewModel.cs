using System;
using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DAL.Entities;

namespace EconomyViewer.MVVM.ViewModel;

public class ItemViewModel : ViewModelBase
{
    private Item? _selectedItem;
    private List<Item> _items;

    public ItemViewModel(List<Item>? items)
    {
        _items = items ?? throw new ArgumentNullException(nameof(items));
    }

    public Item? SelectedCopy { get; private set; }

    public List<Item> Items {
        get => _items;
        set {
            _items = value;
            OnPropertyChanged(nameof(Headers));
        }
    }
    public List<string> Headers => Items?.Select(i => i.Header).ToList() ?? new List<string>();

    public string SelectedHeader {
        get => _selectedItem?.Header ?? string.Empty;
        set {
            if (value == null)
                return;
            if (Headers.Contains(value))
            {
                _selectedItem = Items.Find(item => item.Header == value)!;
                SelectedCopy = _selectedItem.Clone() as Item;
                if (_selectedItem != null)
                    SelectedCopy!.PropertyChanged += (sender, e) => { OnPropertyChanged(nameof(SelectedCopy)); };
                OnPropertyChanged(nameof(SelectedCopy));
            }
        }
    }
}
