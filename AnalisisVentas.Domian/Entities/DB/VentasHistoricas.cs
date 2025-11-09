using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Domian.Entities.DB
{
     

    namespace AnalisisVentas.Domain.Entities.DB
    {
        // Esta clase es un "espejo" de la tabla de la BD Histórica
        // El nombre de cada propiedad debe coincidir con el nombre de la columna en la BD
        public class VentasHistorica // (El archivo es plural, la clase singular)
        {
            public int VentaID { get; set; }
            public int ClienteID { get; set; }
            public DateTime FechaVenta { get; set; }
            public int ProductoID { get; set; }
            public int Cantidad { get; set; }
            public decimal Total { get; set; }
        }
    }
}