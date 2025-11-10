using AnalisisVentas.Application.Result;


namespace AnalisisVentas.Application.Interfaces
{
    public interface IVentasServices
    {
      
        Task<ServiceResult> ProcesarExtraccionAsync();
    }
}