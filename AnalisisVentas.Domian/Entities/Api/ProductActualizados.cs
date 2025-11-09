using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Domian.Entities.Api
{
    namespace AnalisisVentas.Domain.Entities.Api
    {
        // Esta clase es un "espejo" del JSON que recibes de la API
        public class ProductActualizados
        {
            public int ProductoID { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public decimal Precio { get; set; }
        }
    }
}
