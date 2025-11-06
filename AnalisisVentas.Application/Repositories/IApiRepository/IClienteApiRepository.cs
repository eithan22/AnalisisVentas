using AnalisisVentas.Domian.Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Repositories
{
    public  interface IClienteApiRepository
    {
        Task<IEnumerable<ClientesActualizados>> GetClientesAsync();

    }
}
