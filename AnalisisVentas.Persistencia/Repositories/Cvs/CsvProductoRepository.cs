using AnalisisVentas.Application.Interfaces; // La herramienta genérica IExtractor<T>
using AnalisisVentas.Application.Repositories.Csv; // Tu interfaz ICsvProductoRepository
using AnalisisVentas.Domian.Entities.Cvs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Csv
{
    public class CsvProductoRepository : ICsvProductoRepository
    {
        private readonly IExtractor<Product> _csvExtractor;
        private readonly ILogger<CsvProductoRepository> _logger;

        public CsvProductoRepository(
            IExtractor<Product> csvExtractor,
            ILogger<CsvProductoRepository> logger)
        {
            _csvExtractor = csvExtractor;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> ReadFileAsync(string filePath)
        {
            _logger.LogInformation("Repositorio CsvProducto: Delegando extracción...");
            try
            {
                // Delega el trabajo pesado a la herramienta genérica
                var products = await _csvExtractor.ExtractAsync(filePath);
                _logger.LogInformation("Repositorio CsvProducto: Extraídos {Count} productos.", products.Count());
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repositorio CsvProducto: Error al leer el archivo {FilePath}", filePath);
                throw;
            }
        }
    }
}