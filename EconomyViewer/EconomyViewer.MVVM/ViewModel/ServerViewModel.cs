using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.MVVM.ViewModel;

public class ServerViewModel : ViewModelBase
{
    private string? _selectedServerName;
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
            ItemViewModel.Items = SelectedServer?.Items;
        }
    }
    public List<string> Mods => SelectedServer?.Items.Select(s => s.Mod)
                                                     .Distinct()
                                                     .ToList() ?? new List<string>();
    public ItemViewModel ItemViewModel { get; set; }
    public ServerViewModel()
    {
        ApplicationContext.Context.SavedChanges += Context_SavedChanges;
        ItemViewModel = new ItemViewModel() { Items = SelectedServer?.Items };
    }

    private void Context_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        OnPropertyChanged(nameof(ServerNames));
    }
}
