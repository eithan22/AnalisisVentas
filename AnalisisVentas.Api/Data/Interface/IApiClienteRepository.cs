using AnalisisVentas.Api.Data.Entities;

namespace AnalisisVentas.Api.Data.Interface
{
    public interface IApiClienteRepository
    {
        Task<IEnumerable<Customer>> GetClientesAsync();
    }
}