using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinaria_Arca_de_Moe.Models
{
    /// <summary>
    /// Medicamento, vacuna o insumo disponible en el inventario de la clínica.
    /// </summary>
    public class Producto
    {
        public int Id { get; set; }

        // ── Identificación ──────────────────────────────────────────────────

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres.")]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9\-\/\s]+$",
            ErrorMessage = "El código solo puede contener letras, números, guiones y barras.")]
        [Display(Name = "Código / SKU")]
        public string? Codigo { get; set; }

        [StringLength(100)]
        [Display(Name = "Categoría")]
        public string? Categoria { get; set; }   // Ej: Vacuna, Antiparasitario, Antibiótico…

        [StringLength(100)]
        [Display(Name = "Fabricante / Proveedor")]
        public string? Fabricante { get; set; }

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        // ── Inventario ──────────────────────────────────────────────────────

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        [Display(Name = "Stock actual")]
        public int StockActual { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo.")]
        [Display(Name = "Stock mínimo (alerta)")]
        public int StockMinimo { get; set; } = 5;

        [StringLength(30)]
        [Display(Name = "Unidad de medida")]
        public string? UnidadMedida { get; set; }   // Ej: ml, mg, unidad, dosis…

        // ── Precios ─────────────────────────────────────────────────────────

        [Range(0, 999999.99, ErrorMessage = "El precio de costo no puede ser negativo.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Precio de costo (CRC)")]
        public decimal? PrecioCosto { get; set; }

        [Range(0, 999999.99, ErrorMessage = "El precio de venta no puede ser negativo.")]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Precio de venta (CRC)")]
        public decimal? PrecioVenta { get; set; }

        // ── Vencimiento ─────────────────────────────────────────────────────

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        // ── Estado y auditoría ──────────────────────────────────────────────

        [Required]
        [Display(Name = "Estado")]
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        [Display(Name = "Fecha de registro")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        [Display(Name = "Última actualización")]
        public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;

        // ── Relaciones ──────────────────────────────────────────────────────

        public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
        public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();

        // ── Computed ────────────────────────────────────────────────────────

        /// <summary>Indica si el stock está por debajo del mínimo configurado.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [NotMapped]
        public bool StockBajo => StockActual <= StockMinimo;

        /// <summary>Indica si el producto está vencido.</summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [NotMapped]
        public bool Vencido => FechaVencimiento.HasValue && FechaVencimiento.Value.Date < DateTime.Today;
    }
}
