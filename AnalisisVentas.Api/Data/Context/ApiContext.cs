using AnalisisVentas.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.Api.Data.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}