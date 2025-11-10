
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Domian.Entities.Api.AnalisisVentas.Domain.Entities.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json; 



namespace AnalisisVentas.Persistencia.Repositories.Api
{
    public class ClienteApiRepository : IClienteApiRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ClienteApiRepository> _logger;
        private readonly string _endpoint;

        public ClienteApiRepository(
            IHttpClientFactory clientFactory,
            ILogger<ClienteApiRepository> logger,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _endpoint = configuration["ApiSources:ClientesEndpoint"];
        }

       
        public async Task<IEnumerable<ClientesActualizados>> GetClientesActualizadosAsync()
        {
            _logger.LogInformation("Iniciando extracción de API para Clientes desde {Endpoint}...", _endpoint);
            var clientes = new List<ClientesActualizados>();

            try
            {
                using var client = _clientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync(_endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ClientesActualizados>>();
                    if (apiResponse != null)
                    {
                        clientes = apiResponse.ToList();
                    }
                }
                else
                {
                    _logger.LogError("Falló la extracción de API Clientes. Código: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extrayendo clientes de la API en {Endpoint}", _endpoint);
                return new List<ClientesActualizados>();
            }

            _logger.LogInformation("Se extrajeron {Count} clientes de la API.", clientes.Count);
            return clientes;
        }
    }
}