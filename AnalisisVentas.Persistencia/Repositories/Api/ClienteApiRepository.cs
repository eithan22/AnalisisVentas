// Archivo: Persistencia/Repositories/Api/ClienteApiRepository.cs
using AnalisisVentas.Application.Interfaces; // IExtractor<T>
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Domian.Entities.Api.AnalisisVentas.Domain.Entities.Api;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Persistencia.Repositories.Api
{
    public class ClienteApiRepository : IClienteApiRepository
    {
        private readonly IExtractor<ClientesActualizados> _apiExtractor;
        private readonly ILogger<ClienteApiRepository> _logger;

        public ClienteApiRepository(
            IExtractor<ClientesActualizados> apiExtractor, // Inyecta la herramienta genérica
            ILogger<ClienteApiRepository> logger)
        {
            _apiExtractor = apiExtractor;
            _logger = logger;
        }

        // Este es el método de tu interfaz
        public async Task<IEnumerable<ClientesActualizados>> GetClientesActualizados()
        {
            _logger.LogInformation("Repositorio ApiCliente: Delegando extracción a 'clientes'...");

            // Llama a la herramienta genérica pasándole el endpoint específico
            return await _apiExtractor.ExtractAsync("clientes");
        }
    }
}