using AnalisisVentas.Domian.Entities.Api;
using AnalisisVentas.Domian.Entities.Cvs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Repositories.IApiRepository
{
    public interface IProductApiRepositoriy
    {
        Task<IEnumerable<ProductActualizados>> GetProductsAsync();
    }
}
