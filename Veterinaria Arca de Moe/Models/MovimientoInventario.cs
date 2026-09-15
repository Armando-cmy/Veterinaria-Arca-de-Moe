using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Registro de cada entrada, salida o ajuste en el inventario de un producto.
    /// Permite tener trazabilidad completa del stock.
    /// </summary>
    public class MovimientoInventario
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tipo de movimiento")]
        public TipoMovimiento Tipo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        /// <summary>Stock resultante luego del movimiento (calculado al registrar).</summary>
        [Display(Name = "Stock resultante")]
        public int StockResultante { get; set; }

        [StringLength(300)]
        [Display(Name = "Motivo / Referencia")]
        public string? Motivo { get; set; }

        [Display(Name = "Fecha del movimiento")]
        public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow;

        // ── Relaciones ──────────────────────────────────────────────────────

        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        /// <summary>Usuario que registró el movimiento.</summary>
        [Display(Name = "Registrado por")]
        public int? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
}
