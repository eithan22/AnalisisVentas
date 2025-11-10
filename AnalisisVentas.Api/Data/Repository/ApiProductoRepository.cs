using AnalisisVentas.Api.Data.Context;
using AnalisisVentas.Api.Data.Entities;
using AnalisisVentas.Api.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.Api.Data.Repository
{
    public class ApiProductoRepository : IApiProductoRepository
    {
        private readonly ApiContext _context;

        public ApiProductoRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductosAsync()
        {
            // Usa EF Core para leer de la tabla 'Products'
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }
    }
}