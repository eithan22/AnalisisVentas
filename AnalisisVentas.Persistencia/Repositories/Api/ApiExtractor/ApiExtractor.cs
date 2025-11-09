// Archivo: AnalisisVentas.Persistencia/Repositories/Api/ApiExtractor.cs
using AnalisisVentas.Application.Interfaces; // Tu IExtractor<T>
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace AnalisisVentas.Persistencia.Repositories.Api
{
    // 1. La herramienta genérica
    public class ApiExtractor<T> : IExtractor<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiExtractor<T>> _logger;

        public ApiExtractor(HttpClient httpClient, ILogger<ApiExtractor<T>> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<T>> ExtractAsync(string? endpointUrl)
        {
            if (string.IsNullOrEmpty(endpointUrl))
                throw new ArgumentNullException(nameof(endpointUrl));

            _logger.LogInformation("Extrayendo datos de API desde: {Endpoint}", endpointUrl);
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<T>>(endpointUrl);
            return response ?? new List<T>();
        }
    }
}