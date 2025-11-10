using AnalisisVentas.Api.Data.Context;
using AnalisisVentas.Api.Data.Entities;
using AnalisisVentas.Api.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.Api.Data.Repository
{
    // Sigue el patrón de tu profesor (inyecta el DbContext)
    public class ApiClienteRepository : IApiClienteRepository
    {
        private readonly ApiContext _context;

        public ApiClienteRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetClientesAsync()
        {
            // Usa EF Core para leer de la tabla 'Customers'
            return await _context.Customers
                .AsNoTracking()
                .ToListAsync();
        }
    }
}