using AnalisisVentas.Domian.Repository;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace AnalisisVentas.Persistencia.Repositories.Csv
{
    public class CsvFileReaderRepository<TClass> : IFileReaderRepository<TClass> where TClass : class
    {
        private readonly ILogger<CsvFileReaderRepository<TClass>> _logger;

        public CsvFileReaderRepository(ILogger<CsvFileReaderRepository<TClass>> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TClass>> ReadFileAsync(string filePath)
        {
            _logger.LogInformation("Iniciando lectura de CSV: {FilePath}", filePath);
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Archivo CSV no encontrado: {FilePath}", filePath);
                    return new List<TClass>();
                }

                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                
               
                var records = new List<TClass>();
                await foreach (var record in csv.GetRecordsAsync<TClass>())
                {
                    records.Add(record);
                }
                

                _logger.LogInformation("Lectura de {Count} registros completada desde {FilePath}", records.Count, filePath);
                return records;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer CSV: {FilePath}", filePath);
                throw;
            }
        }
    }
}