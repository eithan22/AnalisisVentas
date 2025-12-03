using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Domain.Entities.Dwh.Dimensions;

    [Table("DimProduct", Schema = "Dimension")]
    public class DimProduct
    {
        [Key]
        public int ProductKey { get; set; }

        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string Category { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    }
