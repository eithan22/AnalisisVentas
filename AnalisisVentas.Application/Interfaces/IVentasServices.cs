using AnalisisVentas.Application.Result;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Interfaces
{
    public interface IVentasServices
    {
      
        Task<ServiceResult> ProcesarExtraccionAsync();
    }
}