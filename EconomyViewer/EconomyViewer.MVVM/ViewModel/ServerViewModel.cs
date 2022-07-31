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
    public List<string>? ServerNames => Servers?.Select(s => s.Name)
                                                .ToList();
    public string? SelectedServerName {
        get => _selectedServerName ??= ServerNames!.FirstOrDefault();
        set {
            if (ServerNames!.Contains(value!) == false)
                return;
            _selectedServerName = value;
            OnPropertyChanged(nameof(SelectedServer));
            ItemViewModel = new ItemViewModel(SelectedServer!);
        }
    }

    public ItemViewModel ItemViewModel { get; set; }
    public ServerViewModel()
    {
        ApplicationContext.Context.SavedChanges += Context_SavedChanges;
        ItemViewModel = new ItemViewModel(SelectedServer!);

    }

    private void Context_SavedChanges(object? sender, SavedChangesEventArgs e)
    {
        OnPropertyChanged(nameof(ServerNames));
    }
}
