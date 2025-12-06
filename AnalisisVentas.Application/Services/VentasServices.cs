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

        //private readonly IClienteApiRepository _apiClienteRepo;
        //private readonly IProductApiRepositoriy _apiProductoRepo;
      
        //private readonly IVentasHistoricaRepository _dbVentaRepo;
        private readonly IFileReaderRepository<Customer> _csvClienteRepo;
        private readonly IFileReaderRepository<Product> _csvProductoRepo;
        private readonly IFileReaderRepository<Orders> _csvOrderRepo;
        private readonly IFileReaderRepository<OrderDetail> _csvOrderDetailRepo;

        private readonly IDwhRepository _dwhRepository; 

        public VentasServices(
            ILogger<VentasServices> logger,
            IConfiguration configuration,
           // IClienteApiRepository apiClienteRepo,
          //  IProductApiRepositoriy apiProductoRepo,
           // IVentasHistoricaRepository dbVentaRepo,
            IFileReaderRepository<Customer> csvClienteRepo,
            IFileReaderRepository<Product> csvProductoRepo,
            IFileReaderRepository<Orders> csvOrderRepo,
            IFileReaderRepository<OrderDetail> csvOrderDetailRepo,
            IDwhRepository dwhRepository
            )
        {
            _logger = logger;
            _configuration = configuration;
            //_apiClienteRepo = apiClienteRepo;
            //_apiProductoRepo = apiProductoRepo;
           // _dbVentaRepo = dbVentaRepo;
            _csvClienteRepo = csvClienteRepo;
            _csvProductoRepo = csvProductoRepo;
            _csvOrderRepo = csvOrderRepo;
            _csvOrderDetailRepo = csvOrderDetailRepo;
            _dwhRepository = dwhRepository;
            //NUEVO

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

                /*

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

                 */
                
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



                _logger.LogInformation("===== INICIANDO TRANSFORMACIÓN DE VENTAS =====");
                var ventasUnificadas = CombinarOrdersConDetalles(csvOrders, csvDetails);
                _logger.LogInformation("[T] Transformadas {Count} ventas combinadas (Orders + OrderDetails).", ventasUnificadas.Count());



                // La Transformacion 

                var datosParaCargar = new DimDtos
                {
                    Customers = csvCustomers,
                    Products = csvProducts,
                    Orders = csvOrders,
                     VentasUnificadas = ventasUnificadas
                };

                //  Llamamos al repositorio para guardar
                _logger.LogInformation("Enviando datos al Data Warehouse...");
                var resultado = await _dwhRepository.LoandDimesDataAsync(datosParaCargar);

               
                return resultado;


               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo  en la fase de extracción.");
                return ServiceResult.ErrorResult("Fallo de Extracción: " + ex.Message);
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











