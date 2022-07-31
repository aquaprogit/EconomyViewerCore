using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.Entities;
using EconomyViewer.MVVM.Command;
using EconomyViewer.MVVM.Helper;
using EconomyViewer.Utility;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.MVVM.ViewModel;

public class ItemViewModel : ViewModelBase
{
    private readonly Server _server;
    private Item? _selectedItem;
    private List<Item> _items;
    private Item? _selectedCopy;
    private List<CheckBoxData> _data;

    private RelayCommand _addToSumUpCommand;
    private RelayCommand _removeFromSumUpCommand;
    private RelayCommand _saveEditedItemCommand;
    private RelayCommand _switchAllModsCommand;
    private RelayCommand _addRangeItemsCommand;
    private RelayCommand _addItemCommand;
    private RelayCommand _deleteItemCommand;
    private RelayCommand _clearToSumUpCommand;

    public ItemViewModel(Server server, EventHandler<MVVMMessageBoxEventArgs> handler)
    {
        if (server == null)
            return;
        _server = server;
        MessageBoxRequest += handler;
        Items = server.Items;
        SelectedItem = new Item(true);
        SelectedCopy = new Item(true);
        ToAdd = new Item();

        ToSumUpItems = new ItemList();

    }

    public List<Item> Items {
        get => _items;
        private set {
            _items = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Headers));
        }
    }
    public Item? SelectedItem {
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }
    public Item? SelectedCopy {
        get => _selectedCopy;
        private set {
            _selectedCopy = value;
            OnPropertyChanged();
        }
    }
    public Item ToAdd { get; private set; }
    public List<string> Mods => Items.Select(i => i.Mod)
                                     .Distinct()
                                     .OrderBy(_ => _)
                                     .ToList();
    public List<CheckBoxData> ModsToStates {
        get {
            if (_data != null)
                return _data;
            List<CheckBoxData> coll = new(Mods.Select(c => new CheckBoxData() { Header = c, IsChecked = false }));
            coll.ToList().ForEach(i => i.PropertyChanged += (sender, e) => {
                OnPropertyChanged(nameof(ModsToStates));
                OnPropertyChanged(nameof(Filter));
                OnPropertyChanged(nameof(Headers));
            });
            return _data = coll;
        }
        set {
            _data = value;
            OnPropertyChanged();
        }
    }
    public List<string> Filter => ModsToStates.Where(d => d.IsChecked).Select(d => d.Header).ToList();
    public List<string> Headers => Items.Where(i => Filter.Count == 0 || Filter.Contains(i.Mod))
                                        .Select(i => i.Header)
                                        .ToList() ?? new List<string>();
    public string? SelectedHeader {
        get => _selectedItem?.Header;
        set {
            if (value == null)
            {
                SelectedItem = new Item(true);
                SelectedCopy = new Item(true);
            }
            else if (Headers.Contains(value))
            {
                SelectedItem = Items.Find(item => item.Header == value)!;
                SelectedCopy = (Item)SelectedItem.Clone();
                if (_selectedCopy != null)
                    SelectedCopy.PropertyChanged += (sender, e) => { OnPropertyChanged(nameof(SelectedCopy)); };
            }
        }
    }
    public string FastAddingText { get; set; }
    public ItemList ToSumUpItems { get; set; }
    public string ToSumUpContent => string.Join('\n', ToSumUpItems.Select(i => i.ToString()));
    public int TotalSum => ToSumUpItems.Sum(i => i.Price);

    public RelayCommand AddToSumUpCommand => _addToSumUpCommand ??= new RelayCommand((obj) => {
        ToSumUpItems.Add(SelectedCopy!);
        OnPropertyChanged(nameof(TotalSum));
        OnPropertyChanged(nameof(ToSumUpContent));
        OnPropertyChanged(nameof(ToSumUpItems));
    });
    public RelayCommand RemoveFromSumUpCommand => _removeFromSumUpCommand ??= new RelayCommand((obj) => {
        ToSumUpItems.Remove(ToSumUpItems.Last());
        OnPropertyChanged(nameof(TotalSum));
        OnPropertyChanged(nameof(ToSumUpContent));
    }, (obj) => ToSumUpItems.Any());
    public RelayCommand ClearToSumUpCommand => _clearToSumUpCommand ??= new RelayCommand((obj) => {
        MessageBox_Show(() => {
            ToSumUpItems.Clear();
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
            OnPropertyChanged(nameof(ToSumUpItems));
        }, "Уверены что хотите очистить список сум?", MessageBoxType.Confirmation);
    }, (obj) => ToSumUpItems.Any());
    public RelayCommand SaveEditedItemCommand => _saveEditedItemCommand ??= new RelayCommand(obj => {
        MessageBox_Show(() => {
            ApplicationContext.Context.Update(SelectedItem!);
            ApplicationContext.Context.SaveChanges();
        }, "Уверены что хотите изменить объект?", MessageBoxType.Confirmation,
        () => {
            ApplicationContext.Context.Entry(SelectedItem!).State = EntityState.Unchanged;
            ApplicationContext.Context.SaveChanges();
        });
    }, (obj) => SelectedItem is not null);
    public RelayCommand DeleteItemCommand => _deleteItemCommand ??= new RelayCommand((obj) => {
        MessageBox_Show(() => {
            ApplicationContext.Context.Remove(SelectedItem!);
            ApplicationContext.Context.Entry(_server).State = EntityState.Modified;
            ApplicationContext.Context.SaveChanges();
            Items = Items;
            SelectedItem = null;
        }, "Уверены что хотите удалить объект?", MessageBoxType.Confirmation);
    }, (obj) => SelectedItem is not null);
    public RelayCommand SwitchAllModsCommand => _switchAllModsCommand ??= new RelayCommand((state) => {
        ModsToStates.ForEach(d => d.IsChecked = (bool)state!);
    }, (obj) => SelectedItem is not null);
    public RelayCommand AddItemCommand => _addItemCommand ??= new RelayCommand((obj) => {
        MessageBox_Show(() => {
            ApplicationContext.Context.Servers!.First(s => s.Name == _server.Name).Items.Add(ToAdd);
            ApplicationContext.Context.SaveChanges();
        }, "Предмет успешно добавлен!", MessageBoxType.Notifing);
    }, (obj) => {
        bool isValidates = ToAdd.Equals(Item.FromString(ToAdd.ToString(), ToAdd.Mod));
        return ToAdd.Count > 0 && ToAdd.Price > 0 && isValidates;
    });
    public RelayCommand AddRangeItemsCommand => _addRangeItemsCommand ??= new RelayCommand((obj) => {

    });
}
