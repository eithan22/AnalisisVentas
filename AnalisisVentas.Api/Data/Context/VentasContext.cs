using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.Api.Data.Context
{
    public class VentasContext : DbContext
    {

        public VentasContext(DbContextOptions<VentasContext> options) : base(options)
        {

        }

        
    }
}
