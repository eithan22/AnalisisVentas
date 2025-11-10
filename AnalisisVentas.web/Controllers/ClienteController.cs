using AnalisisVentas.Application.Repositories; 
using Microsoft.AspNetCore.Mvc;


namespace AnalisisVentas.web.Controllers
{
    public class ClienteController : Controller
    {
        
        private readonly IClienteApiRepository _clienteApi;

       
        public ClienteController(IClienteApiRepository clienteApi)
        {
            _clienteApi = clienteApi;
        }

       
        public async Task<IActionResult> Index()
        {
            
            var listaClientes = await _clienteApi.GetClientesActualizadosAsync();

           
            return View(listaClientes);
        }
    }
}