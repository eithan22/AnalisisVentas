
namespace AnalisisVentas.Domain.Entities.DB
{

    public class VentasHistorica
    {
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaVenta { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}