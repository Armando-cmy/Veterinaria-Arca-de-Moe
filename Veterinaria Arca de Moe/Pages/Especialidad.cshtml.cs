using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeterinariaArcaMoe.Pages.Veterinarios
{
    public class EspecialidadModel : PageModel
    {
        public string? Especialidad { get; set; }

        public void OnGet()
        {
            Especialidad = "";
        }
    }
}