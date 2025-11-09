using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories.Csv;
using AnalisisVentas.Domian.Entities.Cvs;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Persistencia.Repositories.Csv
{
    public class CsvOrderRepository : ICsvOrderRepository
    {
        private readonly IExtractor<Orders> _csvExtractor;
        private readonly ILogger<CsvOrderRepository> _logger;

        public CsvOrderRepository(IExtractor<Orders> csvExtractor, ILogger<CsvOrderRepository> logger)
        {
            _csvExtractor = csvExtractor;
            _logger = logger;
        }

        public async Task<IEnumerable<Orders>> ReadFileAsync(string filePath)
        {
            _logger.LogInformation("Repositorio CsvOrder: Delegando extracción...");
            return await _csvExtractor.ExtractAsync(filePath);
        }
    }
}