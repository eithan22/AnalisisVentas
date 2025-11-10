using AnalisisVentas.Api.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AnalisisVentas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly IApiProductoRepository _productoRepo;

        public ProductosApiController(IApiProductoRepository productoRepo)
        {
            _productoRepo = productoRepo;
        }

        // Endpoint: api/ProductosApi/GetProductos
        [HttpGet("GetProductos")]
        public async Task<IActionResult> GetProductos()
        {
            var data = await _productoRepo.GetProductosAsync();
            return Ok(data);
        }
    }
}