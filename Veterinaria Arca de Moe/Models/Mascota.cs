using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    public class Mascota
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es obligatoria.")]
        [StringLength(50)]
        [Display(Name = "Especie")]
        public string Especie { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Raza")]
        public string? Raza { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        [Display(Name = "Propietario")]
        public int PropietarioId { get; set; }

        [ForeignKey("PropietarioId")]
        public Propietario? Propietario { get; set; }

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
