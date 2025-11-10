namespace AnalisisVentas.Domain.Entities.Dwh
{
    public class DimProduct
    {
        public int ProductKey { get; set; }
        public string? ProductName { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}