using AnalisisVentas.Domian.Entities.DB; // O 'Domian'
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

        // 2. Mapea tu entidad 'VentasHistorica'
        public DbSet<VentasHistorica> VentasHistoricas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 3. Configura tu "molde" para que lea de la VISTA que creamos
            modelBuilder.Entity<VentasHistorica>(entity =>
            {
                // ¡Importante! Le dice a EF Core que lea de la VISTA
                entity.ToView("Vw_VentasHistoricas");

                // Le decimos cuál es la llave (ya que las vistas no tienen una por defecto)
                entity.HasKey(e => e.VentaID);
            });
        }
    }
}