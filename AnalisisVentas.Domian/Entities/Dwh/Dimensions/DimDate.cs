using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalisisVentas.Domain.Entities.Dwh.Dimensions;

[Table ("DimDate", Schema = "Dimension")]
public class DimDate
    {

    [Key]
        public int DateKey { get; set; }

        public DateTime FullDate { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }


    public string? DayName { get; set; }   
    public string? MonthName { get; set; } 
}
