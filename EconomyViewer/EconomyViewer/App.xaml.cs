using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EconomyViewer.DB;
using EconomyViewer.Model;
using EconomyViewer.Properties;
using EconomyViewer.Utils;

namespace EconomyViewer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static event Action<string> ServerChanged;

    public App()
    {
        ServerChanged += (server) => {
            Debug.WriteLine("Selected server: " + server);
        };
    }

    public static async void FillContextAsync()
    {
        var allServerNames = ForumDataParser.GetServerNamesToLinks().Keys.ToList() ?? new List<string>();
        var alreadyApplied = ApplicationContext.Context.Servers?.Select(s => s.Name)
                                                                .ToList() ?? new List<string>();

        foreach (string serverName in allServerNames.Except(alreadyApplied))
        {
            ApplicationContext.Context.Servers!.Add(await Task.Run(() => new Server(serverName, ForumDataParser.GetServerItemList(serverName))));
        }
        ApplicationContext.Context.SaveChanges();
    }

    public static string Server {
        get => Settings.Default.DefaultServer;
        set {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value == Settings.Default.DefaultServer)
                return;
            Settings.Default.DefaultServer = value;
            Settings.Default.Save();
            ServerChanged?.Invoke(Server);
        }
    }
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        FillContextAsync();
        MainWindow window = new MainWindow();
        window.Show();
    }
}
