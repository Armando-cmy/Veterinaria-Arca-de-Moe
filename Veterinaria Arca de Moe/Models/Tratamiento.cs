using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Tratamiento o medicamento administrado en una cita o historial médico.
    /// Permite llevar un registro preciso de cada intervención.
    /// </summary>
    public class Tratamiento
    {
        public int Id { get; set; }

        // ── Descripción ─────────────────────────────────────────────────────

        [Required(ErrorMessage = "La descripción del tratamiento es obligatoria.")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "La descripción debe tener entre 2 y 200 caracteres.")]
        [Display(Name = "Tratamiento / Procedimiento")]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Medicamento / Insumo")]
        public string? Medicamento { get; set; }

        [StringLength(100)]
        [Display(Name = "Dosis")]
        public string? Dosis { get; set; }

        [StringLength(100)]
        [Display(Name = "Frecuencia")]
        public string? Frecuencia { get; set; }

        [StringLength(100)]
        [Display(Name = "Duración del tratamiento")]
        public string? Duracion { get; set; }

        [StringLength(500)]
        [Display(Name = "Notas adicionales")]
        public string? Notas { get; set; }

        /// <summary>Costo del tratamiento/procedimiento.</summary>
        [Range(0, 999999.99, ErrorMessage = "El costo no puede ser negativo.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Costo (CRC)")]
        public decimal? Costo { get; set; }

        // ── Relaciones ──────────────────────────────────────────────────────

        /// <summary>Puede estar asociado directamente a una cita.</summary>
        [Display(Name = "Cita")]
        public int? CitaId { get; set; }

        [ForeignKey("CitaId")]
        public Cita? Cita { get; set; }

        /// <summary>O estar asociado a un historial médico.</summary>
        [Display(Name = "Historial médico")]
        public int? HistorialMedicoId { get; set; }

        [ForeignKey("HistorialMedicoId")]
        public HistorialMedico? HistorialMedico { get; set; }

        /// <summary>Producto de inventario consumido (opcional).</summary>
        [Display(Name = "Producto de inventario")]
        public int? ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        // ── Auditoría ───────────────────────────────────────────────────────

        [Display(Name = "Registrado en")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
