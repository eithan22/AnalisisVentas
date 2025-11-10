using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domian.Entities.Cvs;
using AnalisisVentas.Domian.Repository; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Application.Services
{
    
    public class VentasServices : IVentasServices
    {
        
        private readonly IConfiguration _configuration;
        private readonly ILogger<VentasServices> _logger;

        private readonly IClienteApiRepository _apiClienteRepo;
        private readonly IProductApiRepositoriy _apiProductoRepo;
      
        private readonly IVentasHistoricaRepository _dbVentaRepo;
        private readonly IFileReaderRepository<Customer> _csvClienteRepo;
        private readonly IFileReaderRepository<Product> _csvProductoRepo;
        private readonly IFileReaderRepository<Orders> _csvOrderRepo;
        private readonly IFileReaderRepository<OrderDetail> _csvOrderDetailRepo; 

        public VentasServices(
            ILogger<VentasServices> logger,
            IConfiguration configuration,
            IClienteApiRepository apiClienteRepo,
            IProductApiRepositoriy apiProductoRepo,
            IVentasHistoricaRepository dbVentaRepo,
            IFileReaderRepository<Customer> csvClienteRepo,
            IFileReaderRepository<Product> csvProductoRepo,
            IFileReaderRepository<Orders> csvOrderRepo,
            IFileReaderRepository<OrderDetail> csvOrderDetailRepo)
        {
            _logger = logger;
            _configuration = configuration;
            _apiClienteRepo = apiClienteRepo;
            _apiProductoRepo = apiProductoRepo;
            _dbVentaRepo = dbVentaRepo;
            _csvClienteRepo = csvClienteRepo;
            _csvProductoRepo = csvProductoRepo;
            _csvOrderRepo = csvOrderRepo;
            _csvOrderDetailRepo = csvOrderDetailRepo;
        }

       
        public async Task<ServiceResult> ProcesarExtraccionAsync() 
        {
            _logger.LogInformation("===== INICIANDO FASE DE EXTRACCIÓN (TODAS LAS FUENTES) =====");
            try
            {
              
                string basePath = _configuration["CsvSources:BasePath"];
                if (string.IsNullOrEmpty(basePath))
                {
                    _logger.LogError("La ruta 'CsvSources:BasePath' no está configurada en appsettings.json.");
                    return ServiceResult.ErrorResult("Configuración de CsvBasePath no encontrada.");
                }

                //extracción de todas las fuentes


                //Api
                _logger.LogInformation("Extrayendo datos de APIs...");
                var apiCustomers = await _apiClienteRepo.GetClientesActualizadosAsync();
                _logger.LogInformation("[E] Extraídos {Count} clientes de API.", apiCustomers.Count());

                var apiProducts = await _apiProductoRepo.GetProductActualizadosAsync();
                _logger.LogInformation("[E] Extraídos {Count} productos de API.", apiProducts.Count());


                //BD

                _logger.LogInformation("Extrayendo datos de Base de Datos...");
                var dbSales = await _dbVentaRepo.GetVentasHistoricas();
                _logger.LogInformation("[E] Extraídas {Count} ventas históricas de BD.", dbSales.Count());

                
                
                //csv
                
                _logger.LogInformation("Extrayendo datos de CSVs desde: {BasePath}", basePath);

                var csvCustomers = await _csvClienteRepo.ReadFileAsync(Path.Combine(basePath, "customers.csv"));
                _logger.LogInformation("[E] Extraídos {Count} clientes de CSV.", csvCustomers.Count());

                var csvProducts = await _csvProductoRepo.ReadFileAsync(Path.Combine(basePath, "products.csv"));
                _logger.LogInformation("[E] Extraídos {Count} productos de CSV.", csvProducts.Count());


            
                var csvOrders = await _csvOrderRepo.ReadFileAsync(Path.Combine(basePath, "orders.csv"));
                _logger.LogInformation("[E] Extraídas {Count} órdenes de CSV.", csvOrders.Count());

                var csvDetails = await _csvOrderDetailRepo.ReadFileAsync(Path.Combine(basePath, "order_details.csv"));
                _logger.LogInformation("[E] Extraídos {Count} detalles de CSV.", csvDetails.Count());

                _logger.LogInformation("===== FIN DE LA EXTRACCIÓN =====");


                // (La Transformación (T) vendra después)

                return ServiceResult.SuccessResult("Extracción de todas las fuentes completada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo  en la fase de extracción.");
                return ServiceResult.ErrorResult("Fallo de Extracción: " + ex.Message);
            }
        }
    }
}











