using AnalisisVentas.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AnalisisVentas.Application.Interfaces
{
    public interface IVentasServices
    {
        Task<ServiceResult> ProcesarVentasAsync(string filePath);

    }
}
