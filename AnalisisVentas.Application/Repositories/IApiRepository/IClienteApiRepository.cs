using AnalisisVentas.Domian.Entities.Api;

namespace AnalisisVentas.Application.Repositories
{
    
   public interface IClienteApiRepository
    {
        Task<IEnumerable<ClientesActualizados>> GetClientesActualizadosAsync();

    }

    
}
