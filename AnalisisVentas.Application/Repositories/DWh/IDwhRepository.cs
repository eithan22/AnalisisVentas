
using AnalisisVentas.Application.Dtos.DimDto;
using AnalisisVentas.Application.Result;
using System.Runtime.Intrinsics.Arm;

namespace AnalisisVentas.Persistencia.Repositories
     

{
   public interface  IDwhRepository
    {
        Task<ServiceResult> LoandDimesDataAsync(DimDtos dimDtos);


    }
}
