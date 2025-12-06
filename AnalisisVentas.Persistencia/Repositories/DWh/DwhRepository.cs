using AnalisisVentas.Application.Dtos.DimDto;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domain.Entities.Dwh.Dimensions;
using AnalisisVentas.Persistencia.Repositories.DWh.DWContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;


namespace AnalisisVentas.Persistencia.Repositories.DWh
{
    public class DwhRepository : IDwhRepository
    {

        private readonly ILogger<DWHVentasContextcs> _logger;
        private readonly DWHVentasContextcs _context;


        public DwhRepository(DWHVentasContextcs dWHVentasContextcs,
            ILogger<DWHVentasContextcs> logger)
        {
            _context = dWHVentasContextcs;
            _logger = logger;
        }


        //  CARGA DE DIMENSIONES

        public async Task<ServiceResult> LoandDimesDataAsync(DimDtos dimDtos)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                //  limpiar datos antiguos 
                result = await CleanTables();

                if (!result.IsSuccess)
                {
                    this._logger.LogError(result.Message);

                    return result;
                }



                //  CARGA DE CLIENTES (DimCustomer)
                var customers = dimDtos.Customers
                    .Select(c => new
                    {
                        Email = c.Email?.Trim() ?? "Sin Email",
                        Data = c
                    })

                    .DistinctBy(c => c.Email)

                    .Select(c => new DimCustomer
                    {
                        CustomerID = c.Data.CustomerID,
                        FirstName = c.Data.FirstName,
                        LastName = c.Data.LastName,

                        Email = c.Email,
                        Phone = c.Data.Phone ?? "N/A",
                        City = c.Data.City ?? "N/A",
                        Country = c.Data.Country ?? "N/A"
                    }).ToArray();

                await _context.DimCustomers.AddRangeAsync(customers);




                //  CARGA DE PRODUCTOS (DimProduct)
                var products = dimDtos.Products
                    .Select(p => new DimProduct
                    {
                        ProductID = (p.ProductID),
                        ProductName = p.ProductName?.Trim() ?? "Sin Nombre",
                        Category = p.Category ?? "Sin Categoría",
                        Price = p.Price
                    })
                    .DistinctBy(p => p.ProductID)
                    .ToArray();

                await _context.DimProducts.AddRangeAsync(products);





                //  CARGA DE FECHAS (DimDate)

                var cultura = new CultureInfo("es-ES");

                var dates = dimDtos.Orders
                    .Select(o => o.OrderDate)
                    .Distinct()
                    .Select(fe => new DimDate
                    {

                        DateKey = (fe.Year * 10000) + (fe.Month * 100) + fe.Day,
                        FullDate = fe.Date,

                        Day = fe.Day,
                        Month = fe.Month,
                        Year = fe.Year,
                        Quarter = (fe.Month - 1) / 3 + 1,


                        DayName = cultura.TextInfo.ToTitleCase(fe.ToString("dddd", cultura)),
                        MonthName = cultura.TextInfo.ToTitleCase(fe.ToString("MMMM", cultura))
                    }).ToArray();

                await _context.DimDates.AddRangeAsync(dates);





                //  CARGA DE ESTADOS (DimStatus)

                var statuses = dimDtos.Orders
                     .Select(o => o.Status)
                     .Where(s => !string.IsNullOrEmpty(s))
                     .Distinct()
                     .Select((status, index) => new DimStatus
                     {
                         StatusID = index + 1,
                         StatusName = status
                     }).ToArray();

                await _context.DimStatuses.AddRangeAsync(statuses);




                //  CARGA DE DATASOURCE


                var dataSource = new DimDataSource
                {
                    SourceId = 1,
                    SourceType = "CSV",
                    LoadDate = DateTime.Now // Fecha exacta de la carga
                };

                await _context.DimDataSources.AddAsync(dataSource);



                await _context.SaveChangesAsync();
                _logger.LogInformation("Dimensiones guardadas correctamente.");




                // Carga de  FactSales

                var customerMap = customers.ToDictionary(c => c.CustomerID, c => c.CustomerKey);
                var productMap = products.ToDictionary(p => p.ProductID, p => p.ProductKey);
                var statusMap = statuses.ToDictionary(s => s.StatusName, s => s.StatusKey);

                var factSalesList = new List<FactSales>();

                //  Llenar lista usando VentasUnificadas 
                foreach (var venta in dimDtos.VentasUnificadas)
                {
                    // Validamos que los IDs del CSV sean números válidos
                    if (int.TryParse(venta.CustomerID, out int cid) &&
                        int.TryParse(venta.ProductID, out int pid))
                    {
                        

                        bool existeCliente = customerMap.TryGetValue(cid, out int customerKey);
                        bool existeProducto = productMap.TryGetValue(pid, out int productKey);
                        bool existeStatus = statusMap.TryGetValue(venta.Status, out int statusKey);

                        if (existeCliente && existeProducto && existeStatus)
                        {
                            int dateKey = (venta.OrderDate.Year * 10000) + (venta.OrderDate.Month * 100) + venta.OrderDate.Day;

                            factSalesList.Add(new FactSales
                            {
                                CustomerKey = customerKey,
                                ProductKey = productKey,
                                DateKey = dateKey,
                                StatusKey = statusKey,
                                DataSourceKey = dataSource.DataSourceKey,
                                Quantity = venta.Quantity,
                                UnitPrice = venta.UnitPrice,
                                TotalPrice = venta.TotalAmount
                            });
                        }
                        
                    }
                }



                if (factSalesList.Any())
                {
                    await _context.factSales.AddRangeAsync(factSalesList);
                    _logger.LogInformation("tabla fact ventas guardada correctamente.");
                }

                
                await _context.SaveChangesAsync();

                result.IsSuccess = true;

                result.Message = $"Carga completada: {customers.Length} Clientes, {products.Length} Productos, {factSalesList.Count} Ventas.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico cargando ");
                result.IsSuccess = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }




        private async Task<ServiceResult> CleanTables()
        {
            try
            {
              
                await _context.factSales.ExecuteDeleteAsync();
                await _context.DimDataSources.ExecuteDeleteAsync();
                await _context.DimStatuses.ExecuteDeleteAsync();
                await _context.DimDates.ExecuteDeleteAsync();
                await _context.DimProducts.ExecuteDeleteAsync();
                await _context.DimCustomers.ExecuteDeleteAsync();

               
                return ServiceResult.SuccessResult("Tablas limpiadas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error limpiando tablas");
                return ServiceResult.ErrorResult($"Error limpieza: {ex.Message}");
            }
        }
    }
}


       
