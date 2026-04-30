using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using rp_back.Models;

namespace rp_back.Data
{
    public class ReportaSabanaDbContext(DbContextOptions<ReportaSabanaDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Historial> Historiales { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EstadoReporte> Estados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Historial>()
                .HasOne(h => h.Reporte)
                .WithMany()
                .HasForeignKey(h => h.ReporteId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Historial>()
                .HasOne(h => h.Admin)
                .WithMany()
                .HasForeignKey(h => h.AdminId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Reporte>().Property(r => r.Latitud).HasPrecision(9, 6);
            modelBuilder.Entity<Reporte>().Property(r => r.Longitud).HasPrecision(9, 6);
        }

    }
}