using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Cita / consulta veterinaria agendada para una mascota.
    /// </summary>
    public class Cita
    {
        public int Id { get; set; }

        // ── Datos de la cita ────────────────────────────────────────────────

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        [Display(Name = "Fecha y hora")]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        /// <summary>Duración estimada en minutos (15-480).</summary>
        [Range(15, 480, ErrorMessage = "La duración debe estar entre 15 y 480 minutos.")]
        [Display(Name = "Duración (minutos)")]
        public int DuracionMinutos { get; set; } = 30;

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "El motivo debe tener entre 3 y 200 caracteres.")]
        [Display(Name = "Motivo de consulta")]
        public string Motivo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Estado")]
        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

        [StringLength(1000)]
        [Display(Name = "Diagnóstico")]
        public string? Diagnostico { get; set; }

        [StringLength(1000)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        /// <summary>Costo total cobrado por la consulta.</summary>
        [Range(0, 999999.99, ErrorMessage = "El costo no puede ser negativo.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Costo (CRC)")]
        public decimal? Costo { get; set; }

        /// <summary>Indica si la cita fue pagada.</summary>
        [Display(Name = "Pagado")]
        public bool Pagado { get; set; } = false;

        // ── Auditoría ───────────────────────────────────────────────────────

        [Display(Name = "Fecha de creación")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Última actualización")]
        public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;

        // ── Relaciones ──────────────────────────────────────────────────────

        [Required(ErrorMessage = "La mascota es obligatoria.")]
        [Display(Name = "Mascota")]
        public int MascotaId { get; set; }

        [ForeignKey("MascotaId")]
        public Mascota? Mascota { get; set; }

        [Required(ErrorMessage = "El veterinario es obligatorio.")]
        [Display(Name = "Veterinario")]
        public int VeterinarioId { get; set; }

        [ForeignKey("VeterinarioId")]
        public Veterinario? Veterinario { get; set; }

        /// <summary>Historial médico generado al completar la cita (relación 1:0..1).</summary>
        public HistorialMedico? HistorialMedico { get; set; }

        public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();

        // ── Computed ────────────────────────────────────────────────────────

        /// <summary>Fecha estimada de fin de la cita. No se persiste en BD.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [NotMapped]
        public DateTime FechaHoraFin => FechaHora.AddMinutes(DuracionMinutos);
    }
}
