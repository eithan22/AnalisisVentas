using AnalisisVentas.Application.Interfaces; // La herramienta genérica IExtractor<T>
using AnalisisVentas.Application.Repositories.Csv; // Tu interfaz ICsvClienteRepository
using AnalisisVentas.Domian.Entities.Cvs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Csv
{
    public class CsvClienteRepository : ICsvClienteRepository
    {
        private readonly IExtractor<Customer> _csvExtractor;
        private readonly ILogger<CsvClienteRepository> _logger;

        public CsvClienteRepository(
            IExtractor<Customer> csvExtractor,
            ILogger<CsvClienteRepository> logger)
        {
            _csvExtractor = csvExtractor;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> ReadFileAsync(string filePath)
        {
            _logger.LogInformation("Repositorio CsvCliente: Delegando extracción...");
            try
            {
                // Delega el trabajo pesado a la herramienta genérica
                var customers = await _csvExtractor.ExtractAsync(filePath);
                _logger.LogInformation("Repositorio CsvCliente: Extraídos {Count} clientes.", customers.Count());
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repositorio CsvCliente: Error al leer el archivo {FilePath}", filePath);
                throw;
            }
        }
    }
}