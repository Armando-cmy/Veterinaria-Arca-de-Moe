using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Animal registrado en la clínica, asociado a un propietario.
    /// </summary>
    public class Mascota
    {
        public int Id { get; set; }

        // ── Identificación ──────────────────────────────────────────────────

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 1,
            ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres.")]
        [RegularExpression(@"^[\p{L}0-9\s'-]+$",
            ErrorMessage = "El nombre solo puede contener letras, números, espacios, guiones y apóstrofos.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es obligatoria.")]
        [StringLength(50)]
        [RegularExpression(@"^[\p{L}\s]+$",
            ErrorMessage = "La especie solo puede contener letras y espacios.")]
        [Display(Name = "Especie")]
        public string Especie { get; set; } = string.Empty;

        [StringLength(50)]
        [RegularExpression(@"^[\p{L}\s'-]*$",
            ErrorMessage = "La raza solo puede contener letras, espacios, guiones y apóstrofos.")]
        [Display(Name = "Raza")]
        public string? Raza { get; set; }

        [StringLength(30)]
        [Display(Name = "Color / pelaje")]
        public string? Color { get; set; }

        [Display(Name = "Sexo")]
        public Sexo Sexo { get; set; } = Sexo.Desconocido;

        /// <summary>Indica si la mascota está esterilizada.</summary>
        [Display(Name = "Esterilizado/a")]
        public bool Esterilizado { get; set; } = false;

        // ── Datos físicos y médicos ──────────────────────────────────────────

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>Peso en kilogramos. Rango razonable para cualquier especie doméstica.</summary>
        [Range(0.01, 999.99, ErrorMessage = "El peso debe estar entre 0.01 kg y 999.99 kg.")]
        [Column(TypeName = "decimal(6,2)")]
        [Display(Name = "Peso (kg)")]
        public decimal? PesoKg { get; set; }

        [StringLength(500)]
        [Display(Name = "Alergias conocidas")]
        public string? Alergias { get; set; }

        [StringLength(500)]
        [Display(Name = "Enfermedades preexistentes")]
        public string? EnfermedadesPreexistentes { get; set; }

        // ── Número de chip / microchip ───────────────────────────────────────

        [StringLength(20)]
        [RegularExpression(@"^[0-9A-Za-z]{0,20}$",
            ErrorMessage = "El microchip solo puede contener letras y números (máx. 20 caracteres).")]
        [Display(Name = "Nro. de microchip")]
        public string? Microchip { get; set; }

        // ── Estado y auditoría ──────────────────────────────────────────────

        [Required]
        [Display(Name = "Estado")]
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        [Display(Name = "Fecha de registro")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Última actualización")]
        public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;

        // ── Relaciones ──────────────────────────────────────────────────────

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; }

        [ForeignKey("PropietarioId")]
        public Propietario? Propietario { get; set; }

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
        public ICollection<HistorialMedico> Historiales { get; set; } = new List<HistorialMedico>();

        // ── Computed ────────────────────────────────────────────────────────

        /// <summary>Edad calculada en años completos. No se persiste en BD.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [NotMapped]
        public int EdadAnios
        {
            get
            {
                var hoy = DateTime.Today;
                var edad = hoy.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
                return edad;
            }
        }
    }
}
