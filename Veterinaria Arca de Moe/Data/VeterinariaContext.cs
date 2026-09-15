using Microsoft.EntityFrameworkCore;
using Veterinaria_Arca_de_Moe.Models;

namespace Veterinaria_Arca_de_Moe.Data
{
    /// <summary>
    /// Contexto principal de Entity Framework Core para la Veterinaria Arca de Moe.
    /// Todas las relaciones, restricciones e índices se configuran aquí (Fluent API)
    /// para mantener los modelos limpios y la lógica de BD centralizada.
    /// </summary>
    public class VeterinariaContext : DbContext
    {
        public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
            : base(options) { }

        // ── DbSets ──────────────────────────────────────────────────────────

        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<HistorialMedico> HistorialesMedicos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // ── Configuración del modelo ────────────────────────────────────────

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Propietario ──────────────────────────────────────────────────
            modelBuilder.Entity<Propietario>(e =>
            {
                e.ToTable("Propietarios");
                e.HasKey(p => p.Id);

                // Email único (evita duplicados y facilita búsqueda)
                e.HasIndex(p => p.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Propietario_Email");

                // Documento único cuando no es nulo
                e.HasIndex(p => p.Documento)
                    .IsUnique()
                    .HasFilter("[Documento] IS NOT NULL")   // SQLite: solo indexa no-nulos
                    .HasDatabaseName("IX_Propietario_Documento");

                e.Property(p => p.Nombre).HasMaxLength(50).IsRequired();
                e.Property(p => p.Apellidos).HasMaxLength(100).IsRequired();
                e.Property(p => p.Email).HasMaxLength(150).IsRequired();
                e.Property(p => p.Telefono).HasMaxLength(20).IsRequired();
                e.Property(p => p.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ── Mascota ──────────────────────────────────────────────────────
            modelBuilder.Entity<Mascota>(e =>
            {
                e.ToTable("Mascotas");
                e.HasKey(m => m.Id);

                // Microchip único cuando presente
                e.HasIndex(m => m.Microchip)
                    .IsUnique()
                    .HasFilter("[Microchip] IS NOT NULL")
                    .HasDatabaseName("IX_Mascota_Microchip");

                e.Property(m => m.Nombre).HasMaxLength(50).IsRequired();
                e.Property(m => m.Especie).HasMaxLength(50).IsRequired();
                e.Property(m => m.PesoKg).HasColumnType("decimal(6,2)");
                e.Property(m => m.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20);
                e.Property(m => m.Sexo)
                    .HasConversion<string>()
                    .HasMaxLength(15);

                e.HasOne(m => m.Propietario)
                    .WithMany(p => p.Mascotas)
                    .HasForeignKey(m => m.PropietarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Veterinario ──────────────────────────────────────────────────
            modelBuilder.Entity<Veterinario>(e =>
            {
                e.ToTable("Veterinarios");
                e.HasKey(v => v.Id);

                e.HasIndex(v => v.NumeroColegiatura)
                    .IsUnique()
                    .HasFilter("[NumeroColegiatura] IS NOT NULL")
                    .HasDatabaseName("IX_Veterinario_Colegiatura");

                e.Property(v => v.Nombre).HasMaxLength(50).IsRequired();
                e.Property(v => v.Apellidos).HasMaxLength(100).IsRequired();
                e.Property(v => v.Especialidad).HasMaxLength(100).IsRequired();
                e.Property(v => v.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ── Cita ─────────────────────────────────────────────────────────
            modelBuilder.Entity<Cita>(e =>
            {
                e.ToTable("Citas");
                e.HasKey(c => c.Id);

                // Índice compuesto para consultas de disponibilidad del veterinario
                e.HasIndex(c => new { c.VeterinarioId, c.FechaHora })
                    .HasDatabaseName("IX_Cita_Veterinario_Fecha");

                e.Property(c => c.Motivo).HasMaxLength(200).IsRequired();
                e.Property(c => c.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20);
                e.Property(c => c.Costo).HasColumnType("decimal(10,2)");

                e.HasOne(c => c.Mascota)
                    .WithMany(m => m.Citas)
                    .HasForeignKey(c => c.MascotaId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(c => c.Veterinario)
                    .WithMany(v => v.Citas)
                    .HasForeignKey(c => c.VeterinarioId)
                    .OnDelete(DeleteBehavior.Restrict);   // No borrar veterinario con citas

                e.HasOne(c => c.HistorialMedico)
                    .WithOne(h => h.Cita)
                    .HasForeignKey<HistorialMedico>(h => h.CitaId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ── HistorialMedico ──────────────────────────────────────────────
            modelBuilder.Entity<HistorialMedico>(e =>
            {
                e.ToTable("HistorialesMedicos");
                e.HasKey(h => h.Id);

                // Índice para consultas por mascota ordenadas por fecha
                e.HasIndex(h => new { h.MascotaId, h.FechaAtencion })
                    .HasDatabaseName("IX_Historial_Mascota_Fecha");

                e.Property(h => h.Diagnostico).HasMaxLength(1000).IsRequired();
                e.Property(h => h.PesoKg).HasColumnType("decimal(6,2)");
                e.Property(h => h.TemperaturaC).HasColumnType("decimal(4,1)");

                e.HasOne(h => h.Mascota)
                    .WithMany(m => m.Historiales)
                    .HasForeignKey(h => h.MascotaId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(h => h.Veterinario)
                    .WithMany()
                    .HasForeignKey(h => h.VeterinarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Tratamiento ──────────────────────────────────────────────────
            modelBuilder.Entity<Tratamiento>(e =>
            {
                e.ToTable("Tratamientos");
                e.HasKey(t => t.Id);

                e.Property(t => t.Descripcion).HasMaxLength(200).IsRequired();
                e.Property(t => t.Costo).HasColumnType("decimal(10,2)");

                e.HasOne(t => t.Cita)
                    .WithMany(c => c.Tratamientos)
                    .HasForeignKey(t => t.CitaId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                e.HasOne(t => t.HistorialMedico)
                    .WithMany(h => h.Tratamientos)
                    .HasForeignKey(t => t.HistorialMedicoId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);

                e.HasOne(t => t.Producto)
                    .WithMany(p => p.Tratamientos)
                    .HasForeignKey(t => t.ProductoId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });

            // ── Producto ─────────────────────────────────────────────────────
            modelBuilder.Entity<Producto>(e =>
            {
                e.ToTable("Productos");
                e.HasKey(p => p.Id);

                e.HasIndex(p => p.Codigo)
                    .IsUnique()
                    .HasFilter("[Codigo] IS NOT NULL")
                    .HasDatabaseName("IX_Producto_Codigo");

                e.Property(p => p.Nombre).HasMaxLength(150).IsRequired();
                e.Property(p => p.PrecioCosto).HasColumnType("decimal(10,2)");
                e.Property(p => p.PrecioVenta).HasColumnType("decimal(10,2)");
                e.Property(p => p.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20);
            });

            // ── MovimientoInventario ─────────────────────────────────────────
            modelBuilder.Entity<MovimientoInventario>(e =>
            {
                e.ToTable("MovimientosInventario");
                e.HasKey(m => m.Id);

                e.Property(m => m.Tipo)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                e.HasOne(m => m.Producto)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(m => m.ProductoId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(m => m.Usuario)
                    .WithMany(u => u.MovimientosRegistrados)
                    .HasForeignKey(m => m.UsuarioId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });

            // ── Usuario ──────────────────────────────────────────────────────
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("Usuarios");
                e.HasKey(u => u.Id);

                // NombreUsuario y Email únicos (prevención de enumeración de cuentas)
                e.HasIndex(u => u.NombreUsuario)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuario_NombreUsuario");

                e.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Usuario_Email");

                e.Property(u => u.NombreUsuario).HasMaxLength(50).IsRequired();
                e.Property(u => u.Email).HasMaxLength(150).IsRequired();
                e.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();

                // El hash de contraseña nunca debe incluirse en consultas por defecto.
                // Se excluye con proyección explícita en cada query que lo necesite.

                e.Property(u => u.Rol)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                e.HasOne(u => u.Veterinario)
                    .WithOne(v => v.Usuario)
                    .HasForeignKey<Usuario>(u => u.VeterinarioId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);
            });
        }

        // ── Interceptor de auditoría ────────────────────────────────────────

        /// <summary>
        /// Actualiza automáticamente los campos de auditoría (CreadoEn / ActualizadoEn)
        /// antes de guardar cualquier entidad que los implemente.
        /// </summary>
        public override int SaveChanges()
        {
            AplicarAuditoria();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AplicarAuditoria();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AplicarAuditoria()
        {
            var ahora = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                // CreadoEn: solo al insertar
                if (entry.State == EntityState.Added)
                {
                    var creadoEn = entry.Properties
                        .FirstOrDefault(p => p.Metadata.Name == "CreadoEn");
                    if (creadoEn != null && creadoEn.CurrentValue is DateTime dt && dt == default)
                        creadoEn.CurrentValue = ahora;
                }

                // ActualizadoEn: al insertar y al modificar
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var actualizadoEn = entry.Properties
                        .FirstOrDefault(p => p.Metadata.Name == "ActualizadoEn");
                    if (actualizadoEn != null)
                        actualizadoEn.CurrentValue = ahora;
                }
            }
        }
    }
}
