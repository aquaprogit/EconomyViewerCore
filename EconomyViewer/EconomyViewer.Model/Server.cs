using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyViewer.Model;

internal class Server
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Item> Items { get; set; }

    public Server(string name, List<Item> items)
    {
        Name = name;
        Items = items;
    }
    public Server() { }
}
