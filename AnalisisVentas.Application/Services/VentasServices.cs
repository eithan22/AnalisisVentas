using AnalisisVentas.Application.Dtos.DimDto;
using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domain.Entities.Dwh.Dimensions;
using AnalisisVentas.Domian.Entities.Cvs;
using AnalisisVentas.Domian.Repository;
using AnalisisVentas.Persistencia.Repositories;
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

        private readonly IDwhRepository _dwhRepository; 

        public VentasServices(
            ILogger<VentasServices> logger,
            IConfiguration configuration,
            IClienteApiRepository apiClienteRepo,
            IProductApiRepositoriy apiProductoRepo,
            IVentasHistoricaRepository dbVentaRepo,
            IFileReaderRepository<Customer> csvClienteRepo,
            IFileReaderRepository<Product> csvProductoRepo,
            IFileReaderRepository<Orders> csvOrderRepo,
            IFileReaderRepository<OrderDetail> csvOrderDetailRepo,
            IDwhRepository dwhRepository
            )
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
            _dwhRepository = dwhRepository;
            

        }

       
        public async Task<ServiceResult> ProcesarExtraccionAsync() 
        {
            _logger.LogInformation("INICIANDO PROCESO ETL");
            try
            {
                string basePath = _configuration["CsvSources:BasePath"];
                if (string.IsNullOrEmpty(basePath))
                {
                    _logger.LogError("La ruta 'CsvSources:BasePath' no está configurada en appsettings.json.");
                    return ServiceResult.ErrorResult("Configuración de CsvBasePath no encontrada.");
                }

                //  EXTRACCIÓN
                _logger.LogInformation(" EXTRACCION DE DATOS");


                // API
                _logger.LogInformation("Extrayendo de API REST...");
                var apiCustomers = await _apiClienteRepo.GetClientesActualizadosAsync();
                var apiProducts = await _apiProductoRepo.GetProductActualizadosAsync();


                // Base de Datos
                _logger.LogInformation("Extrayendo de Base de Datos...");
                var dbSales = await _dbVentaRepo.GetVentasHistoricas();


                // CSV
                _logger.LogInformation("Extrayendo de archivos CSV: {BasePath}", basePath);
                var csvCustomers = await _csvClienteRepo.ReadFileAsync(Path.Combine(basePath, "customers.csv"));
                var csvProducts = await _csvProductoRepo.ReadFileAsync(Path.Combine(basePath, "products.csv"));
                var csvOrders = await _csvOrderRepo.ReadFileAsync(Path.Combine(basePath, "orders.csv"));
                var csvDetails = await _csvOrderDetailRepo.ReadFileAsync(Path.Combine(basePath, "order_details.csv"));

                
            
                _logger.LogInformation("RESUMEN EXTRACCION:");

                _logger.LogInformation("  API: {ApiC} clientes, {ApiP} productos",
                    apiCustomers.Count(), apiProducts.Count());

                _logger.LogInformation("  Base de Datos: {BD} ventas historicas",
                    dbSales.Count());

                _logger.LogInformation("  CSV: {CsvC} clientes, {CsvP} productos, {CsvO} ordenes, {CsvD} detalles",
                    csvCustomers.Count(), csvProducts.Count(), csvOrders.Count(), csvDetails.Count());



                //  TRANSFORMACIÓN
                _logger.LogInformation(" TRANSFORMACION");

                var ventasUnificadas = CombinarOrdersConDetalles(csvOrders, csvDetails);
                

                //  CARGA
                _logger.LogInformation(" CARGA AL DATA WAREHOUSE");
                var datosParaCargar = new DimDtos
                {
                    Customers = csvCustomers,
                    Products = csvProducts,
                    Orders = csvOrders,
                    VentasUnificadas = ventasUnificadas
                };

                var resultado = await _dwhRepository.LoandDimesDataAsync(datosParaCargar);

                if (resultado.IsSuccess)
                {
                    _logger.LogInformation("PROCESO ETL COMPLETADO: {Message}", resultado.Message);
                }
                else
                {
                    _logger.LogError("ERROR EN CARGA: {Message}", resultado.Message);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error critico en proceso ETL");
                return ServiceResult.ErrorResult($"Error ETL: {ex.Message}");
            }
        }


        private List<VentaDto> CombinarOrdersConDetalles(
           IEnumerable<Orders> orders,
           IEnumerable<OrderDetail> orderDetails)
        {
            var ventasUnificadas = new List<VentaDto>();

            // Convertir orderDetails a diccionario para busqueda muas rapida
            var detallesPorOrder = orderDetails
                .GroupBy(d => d.OrderID)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var order in orders)
            {
                
                if (detallesPorOrder.TryGetValue(order.OrderID, out var detalles))
                {
                    foreach (var detalle in detalles)
                    {
                        //calcular unitprice
                        decimal unitPrice = 0;

                        if (detalle.Quantity > 0)
                        {
                            unitPrice = detalle.TotalPrice / detalle.Quantity;
                        }
                        else
                        {
                            _logger.LogWarning(
                                "OrderID {OrderID}, ProductID {ProductID} tiene Quantity = 0. UnitPrice = 0.",
                                order.OrderID, detalle.ProductID);
                        }

                        ventasUnificadas.Add(new VentaDto
                        {
                            OrderID = order.OrderID.ToString(),
                            CustomerID = order.CustomerID.ToString(),
                            ProductID = detalle.ProductID.ToString(),
                            OrderDate = order.OrderDate,
                            Status = order.Status ?? "Unknown",
                            Quantity = detalle.Quantity,
                            UnitPrice = unitPrice,
                            TotalAmount = detalle.TotalPrice
                        });
                    }
                }
                else
                {
                    _logger.LogWarning("Order {OrderID} no tiene detalles asociados.", order.OrderID);
                }
            }

            return ventasUnificadas;
        }
    }
}











