using AnalisisVentas.Api.Data.Entities;

namespace AnalisisVentas.Api.Data.Interface
{
    public interface IApiProductoRepository
    {
        Task<IEnumerable<Product>> GetProductosAsync();
    }
}