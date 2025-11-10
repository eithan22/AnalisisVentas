
using AnalisisVentas.Domain.Entities.DB;

namespace AnalisisVentas.Application.Repositories.BD
{
    public interface IVentasHistoricaRepository
    {
        Task<IEnumerable<VentasHistorica>> GetVentasHistoricas();

    }
}
