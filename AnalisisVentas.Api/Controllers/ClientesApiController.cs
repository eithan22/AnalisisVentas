using AnalisisVentas.Api.Data.Interface; 
using Microsoft.AspNetCore.Mvc;

namespace AnalisisVentas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
      
        private readonly IApiClienteRepository _clienteRepo;

        public ClientesApiController(IApiClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

      
        [HttpGet("GetClientes")]
        public async Task<IActionResult> GetClientes()
        {
            var data = await _clienteRepo.GetClientesAsync();
            return Ok(data);
        }
    }
}