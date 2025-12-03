using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Services;
using AnalisisVentas.Domian.Repository;
using AnalisisVentas.Persistencia.Repositories;
using AnalisisVentas.Persistencia.Repositories.Csv;
using AnalisisVentas.Persistencia.Repositories.DWh;
using AnalisisVentas.Persistencia.Repositories.DWh.DWContext;
using Microsoft.EntityFrameworkCore;

namespace AnalisisVentas.wksLoadDwh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
        
            //registart las dependencias 

            builder.Services.AddDbContext<DWHVentasContextcs>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("DwhConnection")));


           // .Repositorio Genérico CSV
            builder.Services.AddScoped(typeof(IFileReaderRepository<>), typeof(CsvFileReaderRepository<>));

            //  Repositorio DWH
            builder.Services.AddScoped<IDwhRepository, DwhRepository>();


            // servicio 
            builder.Services.AddScoped<IVentasServices, VentasServices>();

              builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}