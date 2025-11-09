using AnalisisVentas.Application.Dtos;
using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Application.Repositories.Csv;
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domian.Entities.Dwh;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//procesar todos los datos en el dtawehause

namespace AnalisisVentas.Application.Services
{
    public class VentasServices : IVentasServices
    {
        private readonly IClienteApiRepository clienteApiRepository;
        private readonly IProductApiRepositoriy productApiRepository;
        private readonly ICsvOrderDetailRepository csvOrderDetailRepository;
        private readonly ICsvOrderRepository csvOrderRepository;
        private readonly ICsvProductoRepository csvProductoRepository;
        private readonly ICsvClienteRepository csvClienteRepository;
        private readonly IVentasHistoricaRepository ventasHistoricaRepository;
        private readonly ILogger<VentasServices> logger;


        public VentasServices(
         IClienteApiRepository clienteApiRepository,
         IProductApiRepositoriy productApiRepository,
         ICsvOrderRepository csvOrderRepository,
         ICsvOrderDetailRepository csvOrderDetailRepository,
         ICsvProductoRepository csvProductoRepository,
         ICsvClienteRepository csvClienteRepository,
         IVentasHistoricaRepository ventasHistoricaRepository,
         ILogger<VentasServices> logger)

        {
            
            this.clienteApiRepository = clienteApiRepository;
            this.productApiRepository = productApiRepository;
            this.csvOrderRepository = csvOrderRepository;
            this.csvOrderDetailRepository = csvOrderDetailRepository;
            this.csvProductoRepository = csvProductoRepository;
            this.csvClienteRepository = csvClienteRepository;
            this.ventasHistoricaRepository = ventasHistoricaRepository;
            this.logger = logger;
        }

        public async Task<ServiceResult> ProcesarVentasAsync(string filePath)
        {
            try
            {
                // 1️⃣ Cargar datos desde CSV
                 // 1️⃣ Leer todos los archivos
                var clientes = await csvClienteRepository.ReadFileAsync($"{filePath}/Customers.csv");
                var productos = await csvProductoRepository.ReadFileAsync($"{filePath}/Products.csv");
                var orders = await csvOrderRepository.ReadFileAsync($"{filePath}/Orders.csv");
                var detalles = await csvOrderDetailRepository.ReadFileAsync($"{filePath}/OrderDetail.csv");

                var ventas = (from o in orders
                              join d in detalles on o.OrderID equals d.OrderID
                              select new VentaDto
                              {
                                  OrderId = o.OrderID,
                                  CustomerId = o.CustomerID,
                                  OrderDate = o.OrderDate,
                                  ProductId = d.ProductID,
                                  Quantity = d.Quantity,
                                  TotalPrice = d.TotalPrice
                              }).ToList();


                // 3️⃣ Guardar en Data Warehouse
                await ventasHistoricaRepository.GuardarVentasAsync(ventas);

                // 4️⃣ Devolver resultado
                return ServiceResult.SuccessResult("Ventas procesadas correctamente");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error procesando ventas");
                return ServiceResult.ErrorResult("Error procesando ventas: " + ex.Message);
            }
        }



        //aqui tengo que hacer la llamada d cada uno de los metodo del repositorio de persistencia 

    }
}
