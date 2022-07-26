using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.DAL.ViewModel;

public class ServerViewModel : ViewModelBase
{
    private string? _selectedServerName;
    private string? _selectedItemHeader;
    private List<Server>? Servers => ApplicationContext.Context.Servers?.Include(s => s.Items).ToList();
    private Server? SelectedServer => Servers?.FirstOrDefault(s => s.Name == SelectedServerName);
    private List<Item>? Items => SelectedServer?.Items;
    public List<string> ServerNames => Servers?.Select(s => s.Name)
                                               .ToList() ?? new List<string>();
    public List<string> Headers => Items?.Select(i => i.Header).ToList() ?? new List<string>();
    public Item? SelectedItem => Items?.FirstOrDefault(i => i.Header == SelectedItemHeader);
    public string? SelectedServerName {
        get => _selectedServerName ??= ServerNames.FirstOrDefault();
        set {
            _selectedServerName = value;
            OnPropertyChanged(nameof(SelectedServer));
            OnPropertyChanged(nameof(Mods));
        }
    }
    public string? SelectedItemHeader {
        get => _selectedItemHeader ??= Headers.First();
        set {
            _selectedItemHeader = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(ItemStringFormat));
        }
    }
    public List<string> Mods => SelectedServer?.Items.Select(s => s.Mod)
                                                     .Distinct()
                                                     .ToList() ?? new List<string>();
    public string? ItemStringFormat => SelectedItem?.StringFormat;

    public ServerViewModel()
    {
        ApplicationContext.Context.SavedChanges += Context_SavedChanges;
    }

    private void Context_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        OnPropertyChanged(nameof(ServerNames));
    }
}
