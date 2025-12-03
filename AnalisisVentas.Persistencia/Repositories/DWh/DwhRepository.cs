
using AnalisisVentas.Application.Dtos.DimDto;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domain.Entities.Dwh.Dimensions;
using AnalisisVentas.Persistencia.Repositories.DWh.DWContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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


        public async Task<ServiceResult> LoandDimesDataAsync(DimDtos dimDtos)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                //  limpiar datos antiguos 
                var cleanResult = await CleanDimensionTables();
                if (!cleanResult.IsSuccess) return cleanResult;



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




                // CARGA DE FECHAS (DimDate)
                var dates = dimDtos.Orders
                    .Select(o => o.OrderDate)
                    .Distinct()
                    .Select(fe => new DimDate
                    {
                        
                        DateKey = (fe.Year * 10000) + (fe.Month * 100) + fe.Day,
                        IdDate = (fe.Year * 10000) + (fe.Month * 100) + fe.Day,
                        Day = fe.Day,
                        Month = fe.Month,
                        Year = fe.Year,
                        Quarter = (fe.Month - 1) / 3 + 1
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





                // GUARDADO FINAL
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.Message = $"Carga completada: {customers.Length} Clientes, {products.Length} Productos.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico cargando dimensiones");
                result.IsSuccess = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result; 
        }


        private async Task<ServiceResult> CleanDimensionTables()
        {
            try
            {
                // Primero FactSales  luego Dimensiones 
               
                if (_context.factSales.Any())
                {
                    await _context.factSales.ExecuteDeleteAsync();
                }

                await _context.DimProducts.ExecuteDeleteAsync();
                await _context.DimCustomers.ExecuteDeleteAsync();
                await _context.DimDates.ExecuteDeleteAsync();
                await _context.DimStatuses.ExecuteDeleteAsync();
                await _context.DimDataSources.ExecuteDeleteAsync();

               
                await _context.SaveChangesAsync();

                return ServiceResult.SuccessResult("Tablas limpiadas correctamente.");
            }
            catch (Exception ex)
            {
                return ServiceResult.ErrorResult($"Error limpiando tablas: {ex.Message}");
            }
        }


    }
}
    

