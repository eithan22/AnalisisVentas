namespace AnalisisVentas.Domain.Entities.Dwh
{
    public class DimDate
    {
        public int DateKey { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public int Year { get; set; }
    }
}