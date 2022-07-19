using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EconomyViewer.DB;
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
        ApplicationContext context = new ApplicationContext();
        if (context.Items!.Any() == false)
        {
            foreach (string serverName in ForumDataParser.GetServerNamesToLinks().Keys)
            {
                context.Items!.AddRange(await Task.Run(() => ForumDataParser.GetServerItemList(serverName).Select(i => i.AsDto(serverName))));
            }
            context.SaveChanges();
        }
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
