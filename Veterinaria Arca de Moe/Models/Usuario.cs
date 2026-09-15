using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Cuenta de usuario del sistema. Las contraseñas NUNCA se almacenan en texto plano;
    /// se guarda únicamente el hash BCrypt (o PBKDF2) generado al crear/cambiar la contraseña.
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }

        // ── Credenciales ────────────────────────────────────────────────────

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9_\-\.]+$",
            ErrorMessage = "El usuario solo puede contener letras, números, guiones, puntos y guiones bajos.")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Hash de la contraseña (BCrypt / PBKDF2).
        /// NUNCA exponer este campo en vistas ni respuestas de API.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Sal adicional si se usa PBKDF2 personalizado.
        /// Con BCrypt la sal ya está embebida en el hash.
        /// </summary>
        [StringLength(128)]
        public string? PasswordSalt { get; set; }

        // ── Perfil ──────────────────────────────────────────────────────────

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        [StringLength(150)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public RolUsuario Rol { get; set; } = RolUsuario.Recepcionista;

        // ── Seguridad ───────────────────────────────────────────────────────

        /// <summary>Número de intentos fallidos consecutivos de inicio de sesión.</summary>
        [Display(Name = "Intentos fallidos")]
        public int IntentosFallidos { get; set; } = 0;

        /// <summary>Hasta cuándo está bloqueada la cuenta por intentos fallidos.</summary>
        [Display(Name = "Bloqueado hasta")]
        public DateTime? BloqueadoHasta { get; set; }

        /// <summary>Si es false, el usuario no puede iniciar sesión aunque tenga credenciales válidas.</summary>
        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        /// <summary>Token para restablecer contraseña (expirable). Se limpia al usarse.</summary>
        [StringLength(256)]
        public string? TokenResetPassword { get; set; }

        [Display(Name = "Expiración del token de reset")]
        public DateTime? TokenResetExpira { get; set; }

        // ── Auditoría ───────────────────────────────────────────────────────

        [Display(Name = "Fecha de creación")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Último inicio de sesión")]
        public DateTime? UltimoLogin { get; set; }

        // ── Relaciones ──────────────────────────────────────────────────────

        /// <summary>Veterinario al que pertenece esta cuenta (solo si Rol == Veterinario).</summary>
        [Display(Name = "Veterinario asociado")]
        public int? VeterinarioId { get; set; }

        [ForeignKey("VeterinarioId")]
        public Veterinario? Veterinario { get; set; }

        public ICollection<MovimientoInventario> MovimientosRegistrados { get; set; }
            = new List<MovimientoInventario>();

        // ── Computed ────────────────────────────────────────────────────────

        /// <summary>Indica si la cuenta está actualmente bloqueada por intentos fallidos.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [NotMapped]
        public bool EstaBloqueado =>
            BloqueadoHasta.HasValue && BloqueadoHasta.Value > DateTime.UtcNow;
    }
}
