
namespace AnalisisVentas.Domain.Entities.Api
{
    public class ProductActualizados
     {
        //[Key]
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }

        public string Categoria { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

     }
}

