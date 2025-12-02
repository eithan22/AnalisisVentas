
using AnalisisVentas.Application.Dtos.DimDto;
using AnalisisVentas.Application.Result;
using System.Runtime.Intrinsics.Arm;

namespace AnalisisVentas.Persistencia.Repositories
     

{
   public interface  IDataWhRepository
    {
        Task<ServiceResult> LoandDimesDataAsync(DimDtos dimDtos);


    }
}
