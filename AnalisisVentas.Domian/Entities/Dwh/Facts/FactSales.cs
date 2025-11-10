namespace AnalisisVentas.Domain.Entities.Dwh
{
    public class FactSales
    {
        public int SalesKey { get; set; }
        public int? ProductKey { get; set; }
        public int? CustomerKey { get; set; }
        public int? DateKey { get; set; }
        public int? StatusKey { get; set; }
        public int? DataSourceKey { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}