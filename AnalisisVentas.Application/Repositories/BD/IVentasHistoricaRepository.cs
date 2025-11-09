
using AnalisisVentas.Domian.Entities.DB;
using AnalisisVentas.Domian.Entities.DB.AnalisisVentas.Domain.Entities.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Repositories.BD
{
    public interface IVentasHistoricaRepository
    {
        Task<IEnumerable<VentasHistorica>> GetVentasHistoricas();

    }
}
