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
            // Ajusta estas propiedades para que coincidan con los nombres en el JSON
            // (puedes usar [JsonPropertyName("nombre_en_json")] si son muy diferentes)
            public int ClienteID { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string Region { get; set; }
        }
    }
}
