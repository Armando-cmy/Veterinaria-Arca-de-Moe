using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Veterinaria_Arca_de_Moe.Data;
using Veterinaria_Arca_de_Moe.Models;

namespace Veterinaria_Arca_de_Moe.Pages
{
    public class CitasModel : PageModel
    {
        private readonly VeterinariaContext _context;

        public CitasModel(VeterinariaContext context)
        {
            _context = context;
        }

        public List<Cita> Citas { get; set; } = new();
        public List<Mascota> Mascotas { get; set; } = new();
        public List<Veterinario> Veterinarios { get; set; } = new();

        [BindProperty]
        public Cita Cita { get; set; } = new();

        public async Task OnGetAsync()
        {
            Citas = await _context.Citas
                .Include(c => c.Mascota)
                .Include(c => c.Veterinario)
                .ToListAsync();

            Mascotas = await _context.Mascotas.ToListAsync();
            Veterinarios = await _context.Veterinarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Citas = await _context.Citas
                    .Include(c => c.Mascota)
                    .Include(c => c.Veterinario)
                    .ToListAsync();
                Mascotas = await _context.Mascotas.ToListAsync();
                Veterinarios = await _context.Veterinarios.ToListAsync();
                return Page();
            }

            _context.Citas.Add(Cita);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
