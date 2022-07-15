using System;
using System.Diagnostics;
using System.Windows;

using EconomyViewer.Properties;

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
        MainWindow window = new MainWindow();
        window.Show();
    }
}
