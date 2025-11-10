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
        public class ClientesActualizados
        {
            //[Key]
            public int IdCliente { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string Phone { get; set; }

            public string City { get; set; }

            public string Country { get; set; }
        }
    }
}
