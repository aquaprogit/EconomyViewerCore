﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using EconomyViewer.DAL.EF;
using EconomyViewer.DAL.Entities;
using EconomyViewer.Utility.Parser;

namespace EconomyViewer;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App() { }

    public static async Task FillContextAsync()
    {
        var allServerNames = ForumDataParser.GetServerNamesToLinks().Keys.ToList() ?? new List<string>();
        var alreadyApplied = ApplicationContext.Context.Servers?.Select(s => s.Name)
                                                                .ToList() ?? new List<string>();
        List<Task<Server>> servers = new List<Task<Server>>();
        foreach (string serverName in allServerNames.Except(alreadyApplied))
        {
            servers.Add(Task.Run(() => new Server(serverName, ForumDataParser.GetServerItemList(serverName))));
        }

        foreach (var server in servers)
        {
            ApplicationContext.Context.Add(await server);
            Debug.WriteLine("Added");
        }
        await ApplicationContext.Context.SaveChangesAsync();
    }
    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        await FillContextAsync();
        MainWindow window = new MainWindow();
        window.Show();
    }
}
