using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Veterinaria_Arca_de_Moe.Data;
using Veterinaria_Arca_de_Moe.Models;

namespace Veterinaria_Arca_de_Moe.Pages
{
    public class PropietariosModel : PageModel
    {
        private readonly VeterinariaContext _context;

        public PropietariosModel(VeterinariaContext context)
        {
            _context = context;
        }

        public List<Propietario> Propietarios { get; set; } = new();

        [BindProperty]
        public Propietario Propietario { get; set; } = new();

        public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            Propietarios = await _context.Propietarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Propietarios = await _context.Propietarios.ToListAsync();
                return Page();
            }

            _context.Propietarios.Add(Propietario);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var propietario = await _context.Propietarios.FindAsync(id);
            if (propietario != null)
            {
                _context.Propietarios.Remove(propietario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
