using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EconomyViewer.Properties;

namespace EconomyViewer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static event Func<string> ServerChanged;

    public App()
    {
        
    }

    public static string Server
    {
        get => Settings.Default.DefaultServer;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value == Settings.Default.DefaultServer)
                return;
            Settings.Default.DefaultServer = value;
            Settings.Default.Save();

        }
    }
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();
    }
}
