using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DB;
using EconomyViewer.Model;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.ViewModel;

public class ServerViewModel : ViewModelBase
{
    private string? _selectedServerName;
    private List<Server>? Servers => ApplicationContext.Context.Servers?.Include(s => s.Items).ToList();
    private Server? SelectedServer => Servers?.FirstOrDefault(s => s.Name == SelectedServerName);

    public List<string> ServerNames => Servers?.Select(s => s.Name)
                                               .ToList() ?? new List<string>();
    public string SelectedServerName {
        get => _selectedServerName ??= ServerNames.First();
        set {
            _selectedServerName = value;
            OnPropertyChanged(nameof(SelectedServer));
            OnPropertyChanged(nameof(Mods));
        }
    }
    public List<string> Mods => SelectedServer?.Items.Select(s => s.Mod)
                                                     .Distinct()
                                                     .ToList() ?? new List<string>();
}
