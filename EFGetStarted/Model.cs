using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFGetStarted;

public class BloggingContext : DbContext
{
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamWorker> TeamWorkers { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamWorker>().HasKey(o => new
        {
            o.WorkerId,
            o.TeamId
        });
    }
}