using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Registro médico permanente generado al completar una cita.
    /// Actúa como ficha clínica de cada consulta.
    /// </summary>
    public class HistorialMedico
    {
        public int Id { get; set; }

        // ── Datos clínicos ──────────────────────────────────────────────────

        [Required(ErrorMessage = "La fecha de atención es obligatoria.")]
        [Display(Name = "Fecha de atención")]
        [DataType(DataType.DateTime)]
        public DateTime FechaAtencion { get; set; }

        [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
        [StringLength(1000, MinimumLength = 3,
            ErrorMessage = "El diagnóstico debe tener entre 3 y 1000 caracteres.")]
        [Display(Name = "Diagnóstico")]
        public string Diagnostico { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Tratamiento indicado")]
        public string? TratamientoIndicado { get; set; }

        [StringLength(1000)]
        [Display(Name = "Observaciones clínicas")]
        public string? Observaciones { get; set; }

        /// <summary>Peso registrado en la consulta (kg).</summary>
        [Range(0.01, 999.99, ErrorMessage = "El peso debe estar entre 0.01 kg y 999.99 kg.")]
        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Peso en consulta (kg)")]
        public decimal? PesoKg { get; set; }

        /// <summary>Temperatura corporal en ºC.</summary>
        [Range(30.0, 45.0, ErrorMessage = "La temperatura debe estar entre 30 °C y 45 °C.")]
        [Column(TypeName = "decimal(4,1)")]
        [Display(Name = "Temperatura (°C)")]
        public decimal? TemperaturaC { get; set; }

        /// <summary>
        /// Fecha para la próxima consulta de seguimiento.
        /// Permite programar recordatorios de forma sencilla.
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Próxima cita sugerida")]
        public DateTime? ProximaCita { get; set; }

        // ── Auditoría ───────────────────────────────────────────────────────

        [Display(Name = "Registrado en")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        // ── Relaciones ──────────────────────────────────────────────────────

        [Required]
        [Display(Name = "Mascota")]
        public int MascotaId { get; set; }

        [ForeignKey("MascotaId")]
        public Mascota? Mascota { get; set; }

        [Required]
        [Display(Name = "Veterinario")]
        public int VeterinarioId { get; set; }

        [ForeignKey("VeterinarioId")]
        public Veterinario? Veterinario { get; set; }

        /// <summary>Cita que originó este historial (puede ser nula si se crea manualmente).</summary>
        [Display(Name = "Cita de origen")]
        public int? CitaId { get; set; }

        [ForeignKey("CitaId")]
        public Cita? Cita { get; set; }

        public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
    }
}
