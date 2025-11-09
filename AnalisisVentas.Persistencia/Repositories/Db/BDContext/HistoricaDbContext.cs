
using AnalisisVentas.Domian.Entities.DB.AnalisisVentas.Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.Persistencia.Repositories.Db.BDContext
{
    // 1. Este es el Contexto de EF Core para tu BD FUENTE
    public class HistoricaDbContext : DbContext
    {
        public HistoricaDbContext(DbContextOptions<HistoricaDbContext> options)
            : base(options)
        {
        }

        // 2. Mapea tu entidad 'VentasHistorica' a la tabla en la BD
        public DbSet<VentasHistorica> VentasHistoricas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 3. Configura el nombre real de la tabla y la llave primaria
            modelBuilder.Entity<VentasHistorica>(entity =>
            {
                entity.ToTable("VentasHistoricas"); // El nombre de la tabla en SQL
                entity.HasKey(e => e.VentaID);     // La llave primaria
            });
        }
    }
}