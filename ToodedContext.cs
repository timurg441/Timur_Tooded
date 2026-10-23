using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Timur_Tooded;

public class ToodedContext : DbContext
{
    public DbSet<Toode> Toodetabel => Set<Toode>();
    public DbSet<Kategooria> Kategooriatabel => Set<Kategooria>();
    public DbSet<Kasutaja> Kasutajad => Set<Kasutaja>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\MSSQLLocalDB;Database=Tooded_DB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategooria>().ToTable("Kategooriatabel");
        modelBuilder.Entity<Toode>().ToTable("Toodetabel");
        modelBuilder.Entity<Toode>().Property(t => t.Hind).HasPrecision(10, 2);

        modelBuilder.Entity<Kategooria>().HasData(
            new Kategooria { Id = 1, Kategooria_nimetus = "Joogid" },
            new Kategooria { Id = 2, Kategooria_nimetus = "Puu- ja köögiviljad" },
            new Kategooria { Id = 3, Kategooria_nimetus = "Maiustused" },
            new Kategooria { Id = 4, Kategooria_nimetus = "Piimatooted ja munad" },
            new Kategooria { Id = 5, Kategooria_nimetus = "Liha" },
            new Kategooria { Id = 6, Kategooria_nimetus = "Pagaritooted" },
            new Kategooria { Id = 7, Kategooria_nimetus = "Muu" });
    }
}
