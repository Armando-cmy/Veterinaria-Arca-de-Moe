using System.ComponentModel.DataAnnotations;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Persona dueña de una o más mascotas registradas en la clínica.
    /// </summary>
    public class Propietario
    {
        public int Id { get; set; }

       

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

     
        [StringLength(20)]
        [RegularExpression(@"^[A-Za-z0-9\-]+$",
            ErrorMessage = "El documento solo puede contener letras, números y guiones.")]
        [Display(Name = "Documento de identidad")]
        public string? Documento { get; set; }

       

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]{7,20}$",
            ErrorMessage = "Formato de teléfono inválido.")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [StringLength(20)]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        [RegularExpression(@"^\+?[0-9\s\-\(\)]{7,20}$",
            ErrorMessage = "Formato de teléfono inválido.")]
        [Display(Name = "Teléfono alternativo")]
        public string? TelefonoAlt { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        [StringLength(150)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        // ── Estado y auditoría ──────────────────────────────────────────────

        [Required]
        [Display(Name = "Estado")]
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        [Display(Name = "Fecha de registro")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Última actualización")]
        public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;

        // ── Navegación ──────────────────────────────────────────────────────

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();

       

        /// <summary>Nombre completo para mostrar en la UI. No se persiste en BD.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string NombreCompleto => $"{Nombre} {Apellidos}".Trim();
    }
}
