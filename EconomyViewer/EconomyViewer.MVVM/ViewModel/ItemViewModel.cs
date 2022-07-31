using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DAL.Entities;
using EconomyViewer.DAL.EF;
using EconomyViewer.MVVM.Command;
using EconomyViewer.MVVM.Helper;

namespace EconomyViewer.MVVM.ViewModel;

public class ItemViewModel : ViewModelBase
{
    private Item? _selectedItem;
    private List<Item> _items;
    private Item? _selectedCopy;
    private List<CheckBoxData> _data;
    private Server _server;

    public ItemViewModel(Server server)
    {
        if (server == null)
            return;
        _server = server;

        Items = server.Items;
        SelectedItem = new Item();
        SelectedCopy = new Item();

        ToSumUpItems = new ItemList();

        RemoveItemCommand = new RelayCommand((obj) => {
            ToSumUpItems.Remove(ToSumUpItems.Last());
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
        }, (obj) => ToSumUpItems.Any());
        AddItemCommand = new RelayCommand((obj) => {
            ToSumUpItems.Add(SelectedCopy!);
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
            OnPropertyChanged(nameof(ToSumUpItems));
        }, (obj) => true);
        ClearItemsCommand = new RelayCommand((obj) => {
            ToSumUpItems.Clear();
            OnPropertyChanged(nameof(TotalSum));
            OnPropertyChanged(nameof(ToSumUpContent));
            OnPropertyChanged(nameof(ToSumUpItems));
        }, (obj) => true);
        SaveEditChangesCommand = new RelayCommand(obj => {
            ApplicationContext.Context.Update(SelectedItem);
            ApplicationContext.Context.SaveChanges();
        }, (obj) => SelectedItem is not null);
        DeleteItemCommand = new RelayCommand((obj) => {
            ApplicationContext.Context.Remove(SelectedItem);
            ApplicationContext.Context.SaveChanges();
        }, (obj) => SelectedItem is not null);
        SwitchAllModsCommand = new RelayCommand((state) => {
            ModsToStates.ForEach(d => d.IsChecked = (bool)state);
        }, (obj) => SelectedItem is not null);
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
                SelectedCopy = new Item();
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

    public ItemList ToSumUpItems { get; set; }
    public string ToSumUpContent => string.Join('\n', ToSumUpItems.Select(i => i.ToString()));
    public int TotalSum => ToSumUpItems.Sum(i => i.Price);

    public RelayCommand AddItemCommand { get; }
    public RelayCommand RemoveItemCommand { get; }
    public RelayCommand ClearItemsCommand { get; }
    public RelayCommand SaveEditChangesCommand { get; }
    public RelayCommand DeleteItemCommand { get; }
    public RelayCommand SwitchAllModsCommand { get; }
}
