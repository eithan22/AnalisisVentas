

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Domain.Entities.Dwh.Dimensions;

[Table("DimCustomer", Schema = "Dimension")]
public class DimCustomer
{
    [Key]
    public int CustomerKey { get; set; }

    public int CustomerID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}