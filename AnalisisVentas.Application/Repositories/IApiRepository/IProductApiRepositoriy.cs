using AnalisisVentas.Domain.Entities.Api;
using AnalisisVentas.Domian.Entities.Api;

namespace AnalisisVentas.Application.Repositories.IApiRepository
{
    public interface IProductApiRepositoriy
    {
        Task<IEnumerable<ProductActualizados>> GetProductActualizadosAsync();


    }
}
