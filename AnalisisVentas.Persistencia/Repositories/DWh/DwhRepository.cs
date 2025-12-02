
using AnalisisVentas.Application.Dtos.DimDto;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domain.Entities.Dwh;
using AnalisisVentas.Domain.Entities.Dwh.Dimensions;
using AnalisisVentas.Persistencia.Repositories.DWh.DWContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnalisisVentas.Persistencia.Repositories.DWh
{
    public class DwhRepository : IDataWhRepository
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
                // 1. LIMPIEZA: Borrar datos antiguos (Fact primero, luego Dimensiones)
                var cleanResult = await CleanDimensionTables();
                if (!cleanResult.IsSuccess) return cleanResult;



                // 2. CARGA DE CLIENTES (DimCustomer)
                // Tomamos la lista del DTO, quitamos duplicados por Email y mapeamos a la Entidad
                var customers = dimDtos.Customers
                    .Select(c => new
                    {
                        // Normalizamos datos antes de agrupar
                        Email = c.Email?.Trim() ?? "Sin Email",
                        Data = c
                    })
                    .DistinctBy(c => c.Email) // Requiere .NET 6+. Si usas anterior, usa GroupBy
                    .Select(c => new DimCustomer
                    {
                        // Asegúrate que CustomerId en el CSV sea un número válido, o usa TryParse
                        CustomerID = (c.Data.CustomerID),
                        FirstName = c.Data.FirstName,
                        Email = c.Email,
                        Phone = c.Data.Phone ?? "N/A",
                        City = c.Data.City ?? "N/A",
                        Country = c.Data.Country ?? "N/A"
                        // CustomerKey es IDENTITY, SQL Server lo genera solo.
                    }).ToArray();

                await _context.DimCustomers.AddRangeAsync(customers);

                // Guardamos aquí para asegurar que los clientes ya existan si fuera necesario
                // aunque para carga masiva simple podemos guardar al final.


                // 3. CARGA DE PRODUCTOS (DimProduct)
                var products = dimDtos.Products
                    .Select(p => new DimProduct
                    {
                        ProductID = (p.ProductID),
                        ProductName = p.ProductName?.Trim() ?? "Sin Nombre",
                        Category = p.Category ?? "Sin Categoría",
                        Price = p.Price
                    })
                    .DistinctBy(p => p.ProductID) // Evitar duplicados por ID de producto original
                    .ToArray();

                await _context.DimProducts.AddRangeAsync(products);


                // 4. CARGA DE FECHAS (DimDate)
                // Generamos el calendario basándonos en las fechas que vienen en las Órdenes
                var dates = dimDtos.Orders
                    .Select(o => o.OrderDate)
                    .Distinct()
                    .Select(fe => new DimDate
                    {
                        // Lógica de llave inteligente: 20251127 (INT)
                        DateKey = (fe.Year * 10000) + (fe.Month * 100) + fe.Day,
                        IdDate = (fe.Year * 10000) + (fe.Month * 100) + fe.Day,
                        Day = fe.Day,
                        Month = fe.Month,
                        Year = fe.Year,
                        Quarter = (fe.Month - 1) / 3 + 1
                    }).ToArray();

                await _context.DimDates.AddRangeAsync(dates);


                // 5. CARGA DE ESTADOS (DimStatus)
                // Extraemos los estados únicos de las órdenes
                var statuses = dimDtos.Orders
                     .Select(o => o.Status)
                     .Where(s => !string.IsNullOrEmpty(s))
                     .Distinct()
                     .Select((status, index) => new DimStatus
                     {
                         StatusID = index + 1, // ID artificial secuencial
                         StatusName = status
                     }).ToArray();

                await _context.DimStatuses.AddRangeAsync(statuses);



               
                // 6. CARGA DE DATASOURCE (NUEVO)
              
                // Creamos un registro que represente esta carga masiva de CSV
                var dataSource = new DimDataSource
                {
                    SourceId = 1, // Un ID interno arbitrario (1 = CSV, 2 = API, etc.)
                    SourceType = "CSV",
                    LoadDate = DateTime.Now // Fecha exacta de la carga
                };

                await _context.DimDataSources.AddAsync(dataSource);




                // 7. GUARDADO FINAL
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
                // Orden Crítico: Primero FactSales (Hija), luego Dimensiones (Padres)
                // Si borras Dimensiones primero, SQL dará error de Foreign Key
                if (_context.factSales.Any())
                {
                    await _context.factSales.ExecuteDeleteAsync();
                }


                await _context.DimProducts.ExecuteDeleteAsync();
                await _context.DimCustomers.ExecuteDeleteAsync();
                await _context.DimDates.ExecuteDeleteAsync();
                await _context.DimStatuses.ExecuteDeleteAsync();
                await _context.DimDataSources.ExecuteDeleteAsync();

                // Guardamos el estado limpio
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
    

