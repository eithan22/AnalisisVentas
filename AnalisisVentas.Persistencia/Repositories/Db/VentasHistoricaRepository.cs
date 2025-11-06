using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Domian.Entities.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalisisVentas.Persistencia.Repositories.Db
{
    public class VentasHistoricaRepository : IVentasHistoricaRepository
    {
        public Task<IEnumerable<VentasHistoricas>> GetVentasHistoricasDatasAsync()
        {
            throw new NotImplementedException();
        }
    }
}
