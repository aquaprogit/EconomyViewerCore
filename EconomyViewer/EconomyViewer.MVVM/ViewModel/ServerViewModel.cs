using System.Collections.Generic;
using System.Linq;

using EconomyViewer.MVVM.Helper;
using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.Entities;

using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace EconomyViewer.MVVM.ViewModel;

public class ServerViewModel : ViewModelBase
{
    private string? _selectedServerName;
    private ObservableCollection<CheckBoxData> _data;

    private List<Server>? Servers => ApplicationContext.Context.Servers?.Include(s => s.Items).ToList();
    private Server? SelectedServer => Servers?.FirstOrDefault(s => s.Name == SelectedServerName);
    public List<string> ServerNames => Servers?.Select(s => s.Name)
                                               .ToList() ?? new List<string>();
    public string? SelectedServerName {
        get => _selectedServerName ??= ServerNames.FirstOrDefault();
        set {
            _selectedServerName = value;
            OnPropertyChanged(nameof(SelectedServer));
            OnPropertyChanged(nameof(Mods));
            OnPropertyChanged(nameof(Data));
            ItemViewModel.Items = SelectedServer?.Items!;
        }
    }
    public List<string> Mods => SelectedServer?.Items.Select(s => s.Mod)
                                                     .Distinct()
                                                     .ToList() ?? new List<string>();
    public ObservableCollection<CheckBoxData> Data {
        get {
            if (_data != null)
                return _data;
            ObservableCollection<CheckBoxData> coll = new(Mods.Select(c => new CheckBoxData() { Header = c, IsChecked = false }));
            coll.ToList().ForEach(i => i.PropertyChanged += (sender, e) => {
                OnPropertyChanged(nameof(Data));
                OnPropertyChanged(nameof(SelectedMods));
            });
            return _data = coll;
        }
        set {
            _data = value;
            OnPropertyChanged();
        }
    }

    public List<string> SelectedMods => Data.Where(d => d.IsChecked).Select(c => c.Header).ToList();
    public ItemViewModel ItemViewModel { get; set; }
    public ServerViewModel()
    {
        ApplicationContext.Context.SavedChanges += Context_SavedChanges;
        ItemViewModel = new ItemViewModel(SelectedServer?.Items);
    }

    private void Context_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        OnPropertyChanged(nameof(ServerNames));
    }
}
