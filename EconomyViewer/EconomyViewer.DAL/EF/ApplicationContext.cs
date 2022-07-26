using System;
using System.IO;

using EconomyViewer.DAL.Entities;

using Microsoft.EntityFrameworkCore;

namespace EconomyViewer.DAL.EF;

public class ApplicationContext : DbContext
{
    private static ApplicationContext? _instance;
    public DbSet<Server>? Servers { get; set; }
    public DbSet<Item>? Items { get; set; }
    private ApplicationContext()
    {
        Database.EnsureCreated();
    }
    public static ApplicationContext Context => _instance ??= new ApplicationContext();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=" + Path.GetDirectoryName(Environment.ProcessPath) + $@"\economy.db;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Server>()
                    .HasMany(s => s.Items)
                    .WithOne();
    }
}
