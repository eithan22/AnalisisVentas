using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("FactSales", Schema = "Fact")]
public class FactSales
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SalesKey { get; set; }
   
    public int? ProductKey { get; set; }
    public int? CustomerKey { get; set; }
    public int? DateKey { get; set; }
    public int? StatusKey { get; set; }
    public int? DataSourceKey { get; set; }
    public int? Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalPrice { get; set; }
}