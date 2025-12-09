using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Services;

namespace AnalisisVentas.wksLoadDwh
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;


        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    try
                    {
                        
                        using (var scope = _serviceProvider.CreateScope())


                        {
                            var ventasService = scope.ServiceProvider.GetRequiredService<IVentasServices>();
                            

                            _logger.LogInformation("Worker ejecutando: Iniciando extracción...");

                           
                            var resultado = await ventasService.ProcesarExtraccionAsync();

                            if (resultado.IsSuccess)
                                _logger.LogInformation("EXITO: {Msg}", resultado.Message);
                            else
                                _logger.LogError("ERROR: {Msg}", resultado.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error  en el Worker.");
                    }
                }

                _logger.LogInformation("Esperando 12 horas para la siguiente ejecución...");

                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}
