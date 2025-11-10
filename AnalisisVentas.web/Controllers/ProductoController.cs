using AnalisisVentas.Application.Repositories.IApiRepository; 
using Microsoft.AspNetCore.Mvc;

namespace AnalisisVentas.web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductApiRepositoriy _productoApi;

  
        public ProductoController(IProductApiRepositoriy productoApi)
        {
            _productoApi = productoApi;
        }

        public async Task<IActionResult> Index()
        {
            
            var listaProductos = await _productoApi.GetProductActualizadosAsync();

           
            return View(listaProductos);
        }
    }
}