using System;
using System.IO;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.DB;

internal class ApplicationContext : DbContext
{
    public DbSet<ItemDto>? Items { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=" + Path.GetDirectoryName(Environment.ProcessPath) + $@"\economy.db;");
    }
}
