using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Dtos
{
        public class VentaDto
        {
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
            public DateTime OrderDate { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }

            // Campos adicionales
            public string CustomerName { get; set; }
            public string ProductName { get; set; }
        }
    }


