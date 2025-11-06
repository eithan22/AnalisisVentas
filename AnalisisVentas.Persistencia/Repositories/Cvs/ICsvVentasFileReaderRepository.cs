using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Domian.Entities.Cvs;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Cvs
{
    public sealed class CsvVentasFileReaderRepository : ICsvVentasFileReaderRepository
    {
        private readonly string pathFile;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CsvVentasFileReaderRepository> _logger;

        public CsvVentasFileReaderRepository(IConfiguration iconfiguration, ILogger<CsvVentasFileReaderRepository> logger)
        {
            _configuration = iconfiguration;
            _logger = logger;

        }

        public async Task<IEnumerable<VentasHistoricaRepository>> ReadFileAsync(string filePath)
        {
            List<VentasHistoricaRepository> ventas = new List<VentasHistoricaRepository>();

            //oportunidades d emejora con el logger y las validaciones 

            _logger.LogInformation("Starting to read CSV file.");
            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
                
                await foreach (var record in csv.GetRecordsAsync<VentasHistoricaRepository>())
                {
                    ventas.Add(record);
                }


                _logger.LogInformation("CSV file read successfully.");


            }
            catch (Exception ex)
            {
                ventas = null;

                _logger.LogError($"Error reading CSV file: {ex.Message}");
            }
            return ventas;
        }
    }
}
