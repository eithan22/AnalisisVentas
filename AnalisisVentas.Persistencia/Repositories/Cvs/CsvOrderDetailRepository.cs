using AnalisisVentas.Application.Interfaces; // La herramienta genérica IExtractor
using AnalisisVentas.Application.Repositories.Csv; // Tu interfaz ICsvOrderDetailRepository
using AnalisisVentas.Domian.Entities.Cvs;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Persistencia.Repositories.Csv
{
    // 1. Implementa TU interfaz específica
    public class CsvOrderDetailRepository : ICsvOrderDetailRepository
    {
        // 2. Inyecta la "herramienta" genérica
        private readonly IExtractor<OrderDetail> _csvExtractor;
        private readonly ILogger<CsvOrderDetailRepository> _logger;

        public CsvOrderDetailRepository(
            IExtractor<OrderDetail> csvExtractor,
            ILogger<CsvOrderDetailRepository> logger)
        {
            _csvExtractor = csvExtractor;
            _logger = logger;
        }

        // 3. Implementa el método de TU interfaz
        public async Task<IEnumerable<OrderDetail>> ReadFileAsync(string filePath)
        {
            _logger.LogInformation("Repositorio CsvOrderDetail: Delegando extracción...");
            // 4. Delega el trabajo pesado a la herramienta
            return await _csvExtractor.ExtractAsync(filePath);
        }
    }
}