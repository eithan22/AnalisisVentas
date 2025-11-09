// 1. ESTOS SON LOS 'USINGS' CORRECTOS QUE NECESITAS
using AnalisisVentas.Application.Interfaces; // Para IExtractor<T>
using AnalisisVentas.Application.Repositories.IApiRepository; // Para IProductApiRepositoriy
using AnalisisVentas.Domian.Entities.Api.AnalisisVentas.Domain.Entities.Api;
using Microsoft.Extensions.Logging;
using System; // Para Exception
using System.Collections.Generic;
using System.Threading.Tasks;

// 2. ELIMINA EL 'USING' INCORRECTO
// using AnalisisVentas.Domian.Entities.Api.AnalisisVentas.Domain.Entities.Api; 

namespace AnalisisVentas.Persistencia.Repositories.Api
{
    // Implementa TU interfaz específica
    public class ApiProductoRepository : IProductApiRepositoriy
    {
        private readonly IExtractor<ProductActualizados> _apiExtractor;
        private readonly ILogger<ApiProductoRepository> _logger;

        public ApiProductoRepository(
            IExtractor<ProductActualizados> apiExtractor,
            ILogger<ApiProductoRepository> logger)
        {
            _apiExtractor = apiExtractor;
            _logger = logger;
        }

        // 3. Este nombre ('GetProductActualizados') DEBE COINCIDIR
        //    exactamente con el de tu interfaz 'IProductApiRepositoriy'
        public async Task<IEnumerable<ProductActualizados>> GetProductActualizados()
        {
            _logger.LogInformation("Repositorio ApiProducto: Delegando extracción a 'productos'...");
            try
            {
                // Asumimos que el endpoint es 'productos'. Cámbialo si es necesario.
                return await _apiExtractor.ExtractAsync("productos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repositorio ApiProducto: Error al extraer datos.");
                throw;
            }
        }
    }
}