using AnalisisVentas.Application.Interfaces; // Tu interfaz genérica IExtractor<T>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Extractors
{
    // 1. La "herramienta" genérica que usa EF Core
    // TContext debe ser un DbContext (como HistoricaDbContext)
    public class DatabaseExtractor<TEntity, TContext> : IExtractor<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ILogger<DatabaseExtractor<TEntity, TContext>> _logger;

        public DatabaseExtractor(TContext context, ILogger<DatabaseExtractor<TEntity, TContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 2. Implementa el método de IExtractor<T>
        //    (Ignoramos 'parameter' para esta implementación)
        public async Task<IEnumerable<TEntity>> ExtractAsync(string? parameter = null)
        {
            var entityType = typeof(TEntity).Name;
            try
            {
                _logger.LogInformation("Iniciando extracción de BD para: {EntityType}", entityType);

                // 3. Usa Set<T>() para obtener la tabla genérica
                var data = await _context.Set<TEntity>()
                    .AsNoTracking() // Importante para rendimiento en solo lectura
                    .ToListAsync();

                _logger.LogInformation("Extracción de BD completada. Extraídos {Count} registros de {EntityType}", data.Count, entityType);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante extracción de BD para {EntityType}", entityType);
                throw;
            }
        }
    }
}