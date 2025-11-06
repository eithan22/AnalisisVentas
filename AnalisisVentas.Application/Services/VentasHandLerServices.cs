using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Application.Result;
using AnalisisVentas.Domian.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Services
{
    public class VentasHandLerServices : IVentasHandlerServices
    {
        private readonly IClienteApiRepository _clienteApiRepository;
        private readonly IProductApiRepositoriy _productApiRepositoriy;
        private readonly ICsvVentasFileReaderRepository _csvVentasFileReader;
        public VentasHandLerServices(IClienteApiRepository clienteApiRepository,
            IProductApiRepositoriy productApiRepositoriy, 
            ICsvVentasFileReaderRepository csvVentasFileReader)
        {
            _clienteApiRepository = clienteApiRepository;
            _productApiRepositoriy = productApiRepositoriy;
            _csvVentasFileReader = csvVentasFileReader;



        }

        //procesar la data


        public Task<ServiceResult> ProcessVentasDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}
