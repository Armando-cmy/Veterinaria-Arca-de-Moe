using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    public class Cita
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        [Display(Name = "Fecha y hora")]
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(200)]
        [Display(Name = "Motivo")]
        public string Motivo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Estado de cita")]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(500)]
        [Display(Name = "Diagnóstico")]
        public string? Diagnostico { get; set; }

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
    }
}
