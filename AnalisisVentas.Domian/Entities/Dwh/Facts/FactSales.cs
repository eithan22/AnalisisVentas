using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table("FactSales", Schema = "Fact")]
public class FactSales
{
    [Key]

    public int SalesKey { get; set; }
    public int SalesId { get; set; }
    public int? ProductKey { get; set; }
    public int? CustomerKey { get; set; }
    public int? DateKey { get; set; }
    public int? StatusKey { get; set; }
    public int? DataSourceKey { get; set; }
    public int? Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? TotalPrice { get; set; }
}