// Archivo: AnalisisVentas.Api/Controllers/ClientesApiController.cs
using AnalisisVentas.Api.Data.Interface; // ¡Importa tu interfaz!
using Microsoft.AspNetCore.Mvc;

namespace AnalisisVentas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesApiController : ControllerBase
    {
        // 1. Inyecta la INTERFAZ (el contrato)
        private readonly IApiClienteRepository _clienteRepo;

        public ClientesApiController(IApiClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        // 2. Crea el endpoint que tu Worker llamará
        // Endpoint: api/ClientesApi/GetClientes
        [HttpGet("GetClientes")]
        public async Task<IActionResult> GetClientes()
        {
            var data = await _clienteRepo.GetClientesAsync();
            return Ok(data);
        }
    }
}