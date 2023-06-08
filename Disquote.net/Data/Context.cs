using System;
using Microsoft.EntityFrameworkCore;

namespace Disquote.net.Data;

public class Context : DbContext
{
    public DbSet<Quote> Quotes { get; set; }

    private string DbPath { get; }

    public Context()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "disquote.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quote>()
            .HasKey(q => new { q.GuildId, q.Id });
    }
}