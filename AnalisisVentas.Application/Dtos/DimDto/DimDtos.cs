using AnalisisVentas.Domian.Entities.Cvs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Dtos.DimDto
{
    public class DimDtos
    {
        
        public IEnumerable<Customer> Customers { get; set; } = new List<Customer>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        //  generar las Dimensiones de fecha y estado con ordenes
        public IEnumerable<Orders> Orders { get; set; } = new List<Orders>();

        //utilizdo para cargar la tabla de hechos
        public IEnumerable<VentaDto> VentasUnificadas { get; set; } = new List<VentaDto>();
    }

}

