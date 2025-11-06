using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Domian.Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Api
{
    public class ClienteApiRepository : IClienteApiRepository
    {
       

        public Task<IEnumerable <ClientesActualizados>> GetClientesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
