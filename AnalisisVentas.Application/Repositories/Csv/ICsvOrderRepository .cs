using AnalisisVentas.Domian.Entities.Cvs;
using AnalisisVentas.Domian.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Application.Repositories.Csv
{
    public interface ICsvOrderRepository : IFileReaderRepository<Orders>
    {
    }
}
