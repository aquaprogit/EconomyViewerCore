using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.DB;

internal class ApplicationContext : DbContext
{
    private static ApplicationContext? _instance;
    public DbSet<ItemDto>? Items { get; set; }
    private ApplicationContext()
    {
        Database.EnsureCreated();
    }
    public static ApplicationContext Context => _instance ??= new ApplicationContext();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=" + Path.GetDirectoryName(Environment.ProcessPath) + $@"\economy.db;");
    }
}
