using System.Collections.Generic;

namespace EconomyViewer.DAL.Entities;

public class Server
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
