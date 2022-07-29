using System;
using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DAL.Entities;
using EconomyViewer.MVVM.Command;

namespace EconomyViewer.MVVM.ViewModel;

public class ItemViewModel : ViewModelBase
{
    private Item? _selectedItem;
    private List<Item> _items;
    private Item? _selectedCopy;

    public ItemViewModel(List<Item>? items)
    {
        _items = items ?? throw new ArgumentNullException(nameof(items));
        ToSumUpItems = new ItemList();
        RemoveItemCommand = new RelayCommand((obj) => {
            ToSumUpItems.Remove(ToSumUpItems.Last());
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
        },
        (obj) => ToSumUpItems.Any());
        AddItemCommand = new RelayCommand((obj) => {
            ToSumUpItems.Add(SelectedCopy!);
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
        },
        (obj) => true);
        ClearItemsCommand = new RelayCommand((obj) => {
            ToSumUpItems.Clear();
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
        }, (obj) => true);
    }

    public Item? SelectedCopy {
        get => _selectedCopy;
        private set {
            _selectedCopy = value;
            OnPropertyChanged();
        }
    }
    public List<Item> Items {
        get => _items;
        set {
            _items = value;
            ClearItemsCommand.Execute(this);
            OnPropertyChanged(nameof(SelectedCopy));
            OnPropertyChanged(nameof(Headers));
        }
    }
    public List<string> Headers => Items?.Select(i => i.Header).ToList() ?? new List<string>();
    public string SelectedHeader {
        get => _selectedItem?.Header ?? string.Empty;
        set {
            if (value == null)
            {
                SelectedCopy = new Item();
            }
            else if (Headers.Contains(value))
            {
                _selectedItem = Items.Find(item => item.Header == value)!;
                SelectedCopy = _selectedItem.Clone() as Item;
                if (_selectedItem != null)
                    SelectedCopy!.PropertyChanged += (sender, e) => { OnPropertyChanged(nameof(SelectedCopy)); };
            }
        }
    }
    public ItemList ToSumUpItems { get; set; }
    public string ToSumUpContent => string.Join('\n', ToSumUpItems.Select(i => i.ToString()));
    public int TotalSum => ToSumUpItems.Sum(i => i.Price);
    public RelayCommand AddItemCommand { get; }
    public RelayCommand RemoveItemCommand { get; }
    public RelayCommand ClearItemsCommand { get; }
}
