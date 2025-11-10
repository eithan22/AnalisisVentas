using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Domain.Entities.DB;
using AnalisisVentas.Persistencia.Repositories.Db.BDContext; 
using Microsoft.EntityFrameworkCore; 


namespace AnalisisVentas.Persistencia.Repositories.Db
{
   
    public class VentasHistoricaRepository : IVentasHistoricaRepository
    {
      
        private readonly HistoricaDbContext _context;

        public VentasHistoricaRepository(HistoricaDbContext context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<VentasHistorica>> GetVentasHistoricas()
        {
          
            return await _context.VentasHistoricas
                .AsNoTracking() 
                .ToListAsync();
        }
    }
}