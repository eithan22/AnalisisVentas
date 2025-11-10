
using AnalisisVentas.Domain.Entities.DB;
using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.Persistencia.Repositories.Db.BDContext
{
   
    public class HistoricaDbContext : DbContext
    {
        public HistoricaDbContext(DbContextOptions<HistoricaDbContext> options)
            : base(options)
        {
        }

        public DbSet<VentasHistorica> VentasHistoricas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<VentasHistorica>(entity =>
            {
                // EF Core que lea de la vista

                entity.ToView("Vw_VentasHistoricas");
                entity.HasKey(e => e.VentaID);
            });
        }
    }
}