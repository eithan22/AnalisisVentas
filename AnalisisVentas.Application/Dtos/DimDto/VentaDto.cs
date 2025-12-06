using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Dtos.DimDto
{
    public class VentaDto
    {
        
        public string OrderID { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }


        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
