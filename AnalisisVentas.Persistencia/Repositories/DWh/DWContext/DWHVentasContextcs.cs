using AnalisisVentas.Domain.Entities.Dwh;
using AnalisisVentas.Domain.Entities.Dwh.Dimensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalisisVentas.Persistencia.Repositories.DWh.DWContext
{
    public class DWHVentasContextcs : DbContext
    {
        public DWHVentasContextcs(DbContextOptions<DWHVentasContextcs> options)
            : base(options)
        {
        }

        public DbSet<DimCustomer> DimCustomers { get; set; }
        public DbSet<DimDate> DimDates { get; set; }
        public DbSet<DimStatus> DimStatuses { get; set; }

        public DbSet<DimProduct> DimProducts { get; set; }

        public DbSet<DimDataSource> DimDataSources { get; set; }

        public DbSet<FactSales> factSales { get; set; }




    }
}
