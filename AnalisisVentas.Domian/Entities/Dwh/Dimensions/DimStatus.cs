using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Domain.Entities.Dwh.Dimensions;
[Table("DimStatus", Schema = "Dimension")]

public class DimStatus
    {
    [Key]
        public int StatusKey { get; set; }

        public int StatusID { get; set; }
        public string? StatusName { get; set; }
    }
