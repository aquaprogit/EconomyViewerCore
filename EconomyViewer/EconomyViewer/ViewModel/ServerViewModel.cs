using System.Collections.Generic;
using System.Linq;

using EconomyViewer.DB;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.ViewModel;

public class ServerViewModel : ViewModelBase
{
    private string _selectedServer;

    public List<string> Servers => ApplicationContext.Context.Items?.Select(c => c.ServerName)
                                                                    .Distinct()
                                                                    .OrderBy(s => s)
                                                                    .ToList() ?? new List<string>();

    public string SelectedServer {
        get => _selectedServer ??= Servers.First();
        set {
            _selectedServer = value;
            OnPropertyChanged(nameof(Mods));
        }
    }

    public List<string> Mods => ApplicationContext.Context.Items?.Where(c => c.ServerName == SelectedServer)
                                                                 .Select(c => c.Mod)
                                                                 .Distinct()
                                                                 .ToList() ?? new List<string>();
}
