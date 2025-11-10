
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Domain.Entities.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace AnalisisVentas.Persistencia.Repositories.Api
{
    public class ProductoApiRepository : IProductApiRepositoriy
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ProductoApiRepository> _logger;
        private readonly string _endpoint;

        public ProductoApiRepository(
            IHttpClientFactory clientFactory,
            ILogger<ProductoApiRepository> logger,
            IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _endpoint = configuration["ApiSources:ProductosEndpoint"];
        }

        public async Task<IEnumerable<ProductActualizados>> GetProductActualizadosAsync()
        {
            _logger.LogInformation("Iniciando extracción de API para Productos desde {Endpoint}...", _endpoint);
            var productos = new List<ProductActualizados>();

            try
            {
                using var client = _clientFactory.CreateClient("ApiClient");
                var response = await client.GetAsync(_endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ProductActualizados>>();
                    if (apiResponse != null)
                    {
                        productos = apiResponse.ToList();
                    }
                }
                else
                {
                    _logger.LogError("Fallo la extracción de API Productos. Codigo: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extrayendo productos de la API en {Endpoint}", _endpoint);
                return new List<ProductActualizados>();
            }

            _logger.LogInformation("Se extrajeron {Count} productos de la API.", productos.Count);
            return productos;
        }
    }
}