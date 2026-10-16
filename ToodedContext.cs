using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timur_Tooded
{
    using Microsoft.EntityFrameworkCore;

    public class ToodedContext : DbContext
    {
        public DbSet<Toode> Toodetabel { get; set; }
        public DbSet<Kategooria> Kategooriatabel { get; set; }
        public DbSet<Kasutaja> Kasutajad { get; set; }
        public DbSet<Klient> Kliendid { get; set; }

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
            modelBuilder.Entity<Klient>().ToTable("Kliendid");
            modelBuilder.Entity<Klient>().Property(k => k.Boonus).HasPrecision(10, 2);
        }
    }
}
