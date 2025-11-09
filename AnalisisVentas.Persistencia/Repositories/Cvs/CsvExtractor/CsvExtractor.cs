using AnalisisVentas.Application.Interfaces; // Tu interfaz genérica
using CsvHelper; // Asegúrate de tener el paquete NuGet
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Csv
{
    // La implementación genérica
    public class CsvExtractor<T> : IExtractor<T> where T : class
    {
        private readonly ILogger<CsvExtractor<T>> _logger;

        public CsvExtractor(ILogger<CsvExtractor<T>> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<T>> ExtractAsync(string? parameter)
        {
            // El 'parameter' es la ruta del archivo (ej. "C:\datos\orders.csv")
            if (string.IsNullOrEmpty(parameter) || !File.Exists(parameter))
            {
                _logger.LogError("CSV file not found at path: {FilePath}", parameter);
                throw new FileNotFoundException($"CSV file not found at path: {parameter}");
            }

            var result = new List<T>();
            try
            {
                _logger.LogInformation("Starting CSV extraction from {FilePath}", parameter);

                using var reader = new StreamReader(parameter);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                await foreach (var record in csv.GetRecordsAsync<T>())
                {
                    result.Add(record);
                }

                _logger.LogInformation("CSV extraction completed. Extracted {Count} records from {FilePath}", result.Count, parameter);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CSV extraction from {FilePath}", parameter);
                throw;
            }
        }
    }
}