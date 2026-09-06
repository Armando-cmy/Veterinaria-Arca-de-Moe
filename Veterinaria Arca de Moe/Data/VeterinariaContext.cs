using Microsoft.EntityFrameworkCore;
using Veterinaria_Arca_de_Moe.Models;

namespace Veterinaria_Arca_de_Moe.Data
{
    public class VeterinariaContext : DbContext
    {
        public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
            : base(options)
        {
        }

        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mascota>()
                .HasOne(m => m.Propietario)
                .WithMany(p => p.Mascotas)
                .HasForeignKey(m => m.PropietarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Mascota)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.MascotaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Veterinario)
                .WithMany(v => v.Citas)
                .HasForeignKey(c => c.VeterinarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
