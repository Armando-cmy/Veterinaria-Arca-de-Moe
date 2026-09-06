using System.ComponentModel.DataAnnotations;

namespace Veterinaria_Arca_de_Moe.Models
{
    public class Veterinario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100)]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(100)]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo";

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
