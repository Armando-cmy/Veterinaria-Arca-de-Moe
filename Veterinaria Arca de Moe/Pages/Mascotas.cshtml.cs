using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Veterinaria_Arca_de_Moe.Data;
using Veterinaria_Arca_de_Moe.Models;

namespace Veterinaria_Arca_de_Moe.Pages
{
    public class MascotasModel : PageModel
    {
        private readonly VeterinariaContext _context;

        public MascotasModel(VeterinariaContext context)
        {
            _context = context;
        }

        public List<Mascota> Mascotas { get; set; } = new();
        public List<Propietario> Propietarios { get; set; } = new();

        [BindProperty]
        public Mascota Mascota { get; set; } = new();

        public async Task OnGetAsync()
        {
            Mascotas = await _context.Mascotas
                .Include(m => m.Propietario)
                .ToListAsync();

            Propietarios = await _context.Propietarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Mascotas = await _context.Mascotas.Include(m => m.Propietario).ToListAsync();
                Propietarios = await _context.Propietarios.ToListAsync();
                return Page();
            }

            _context.Mascotas.Add(Mascota);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
