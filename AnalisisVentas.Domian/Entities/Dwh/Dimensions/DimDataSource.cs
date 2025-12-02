
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Domain.Entities.Dwh.Dimensions;

[Table("DimDataSource", Schema = "Dimension")]
public class DimDataSource
{
    [Key]
    public int DataSourceKey { get; set; }

    public int SourceId { get; set; }
    public string? SourceType { get; set; }
    public DateTime? LoadDate { get; set; }
}
