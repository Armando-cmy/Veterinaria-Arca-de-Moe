using System.ComponentModel.DataAnnotations;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Profesional veterinario que atiende citas en la clínica.
    /// </summary>
    public class Veterinario
    {
        public int Id { get; set; }

        // ── Datos personales ────────────────────────────────────────────────

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[\p{L}\s'-]+$",
            ErrorMessage = "El nombre solo puede contener letras, espacios, guiones y apóstrofos.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[\p{L}\s'-]+$",
            ErrorMessage = "Los apellidos solo pueden contener letras, espacios, guiones y apóstrofos.")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = string.Empty;

        // ── Datos profesionales ─────────────────────────────────────────────

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(100)]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; } = string.Empty;

        /// <summary>Número de colegiatura / licencia profesional. Único por veterinario.</summary>
        [StringLength(30)]
        [RegularExpression(@"^[A-Za-z0-9\-\/]+$",
            ErrorMessage = "El número de colegiatura solo puede contener letras, números, guiones y barras.")]
        [Display(Name = "Nro. de colegiatura")]
        public string? NumeroColegiatura { get; set; }

        // ── Contacto ────────────────────────────────────────────────────────

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]{7,20}$",
            ErrorMessage = "Formato de teléfono inválido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        [StringLength(150)]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        // ── Horario ─────────────────────────────────────────────────────────

        /// <summary>
        /// Días laborables almacenados como flags de bits (lunes=1, martes=2, …, domingo=64).
        /// Permite consultar disponibilidad sin tablas adicionales para casos sencillos.
        /// </summary>
        [Display(Name = "Días laborables")]
        public byte DiasLaborables { get; set; } = 0b0011111; // Lun-Vie por defecto

        [DataType(DataType.Time)]
        [Display(Name = "Hora de entrada")]
        public TimeSpan? HoraEntrada { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Hora de salida")]
        public TimeSpan? HoraSalida { get; set; }

        // ── Estado y auditoría ──────────────────────────────────────────────

        [Required]
        [Display(Name = "Estado")]
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        [Display(Name = "Fecha de registro")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Última actualización")]
        public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;

        // ── Relaciones ──────────────────────────────────────────────────────

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();

        /// <summary>Cuenta de usuario del sistema asociada a este veterinario (opcional).</summary>
        public Usuario? Usuario { get; set; }

        // ── Computed ────────────────────────────────────────────────────────

        [System.Text.Json.Serialization.JsonIgnore]
        public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();
    }
}
