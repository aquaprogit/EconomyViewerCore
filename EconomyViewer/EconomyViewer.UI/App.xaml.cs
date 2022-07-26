using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EconomyViewer.Utility.Parser;
using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.Entities;

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
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        FillContextAsync();
        MainWindow window = new MainWindow();
        window.Show();
    }
}
