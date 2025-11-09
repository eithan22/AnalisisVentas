using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Domian.Entities.DB.AnalisisVentas.Domain.Entities.DB;
using AnalisisVentas.Persistencia.Repositories.Db.BDContext;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.Db
{
    public class VentasHistoricaRepository : IVentasHistoricaRepository
    {
        private readonly IExtractor<VentasHistorica> _dbExtractor;
        private readonly ILogger<VentasHistoricaRepository> _logger;

        public VentasHistoricaRepository(
            IExtractor<VentasHistorica> dbExtractor,
            ILogger<VentasHistoricaRepository> logger)
        {
            _dbExtractor = dbExtractor;
            _logger = logger;
        }

        // El nombre "GetVentasHistoricas" coincide con tu interfaz
        public async Task<IEnumerable<VentasHistorica>> GetVentasHistoricas()
        {
            _logger.LogInformation("Repositorio VentasHistorica: Delegando extracción a DatabaseExtractor...");
            try
            {
                // Delega el trabajo pesado (el parámetro es null)
                return await _dbExtractor.ExtractAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repositorio VentasHistorica: Error al extraer datos.");
                throw;
            }
        }
    }
}