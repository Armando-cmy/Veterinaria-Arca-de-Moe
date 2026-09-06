using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Veterinaria_Arca_de_Moe.Data;
using Veterinaria_Arca_de_Moe.Models;

namespace Veterinaria_Arca_de_Moe.Pages
{
    public class VeterinariosModel : PageModel
    {
        private readonly VeterinariaContext _context;

        public VeterinariosModel(VeterinariaContext context)
        {
            _context = context;
        }

        public List<Veterinario> Veterinarios { get; set; } = new();

        [BindProperty]
        public Veterinario Veterinario { get; set; } = new();

        public async Task OnGetAsync()
        {
            Veterinarios = await _context.Veterinarios.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Veterinarios = await _context.Veterinarios.ToListAsync();
                return Page();
            }

            _context.Veterinarios.Add(Veterinario);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario != null)
            {
                _context.Veterinarios.Remove(veterinario);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
