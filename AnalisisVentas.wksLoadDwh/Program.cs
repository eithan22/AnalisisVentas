using AnalisisVentas.Application.Interfaces;
using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Application.Repositories.BD;
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Application.Services;
using AnalisisVentas.Domian.Repository;
using AnalisisVentas.Persistencia.Repositories;
using AnalisisVentas.Persistencia.Repositories.Api;
using AnalisisVentas.Persistencia.Repositories.Csv;
using AnalisisVentas.Persistencia.Repositories.Db;
using AnalisisVentas.Persistencia.Repositories.Db.BDContext;
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

           
            builder.Services.AddDbContext<DWHVentasContextcs>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DwhConnection")));

            builder.Services.AddDbContext<HistoricaDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HistoricaConnection")));

            // API
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                var baseUrl = builder.Configuration["ApiSources:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            //repositoies

            //  CSV
            builder.Services.AddScoped(typeof(IFileReaderRepository<>), typeof(CsvFileReaderRepository<>));

            //  API 
            builder.Services.AddScoped<IClienteApiRepository, ClienteApiRepository>();
            builder.Services.AddScoped<IProductApiRepositoriy, ProductoApiRepository>();

            //  BASE DE DATOS 
            builder.Services.AddScoped<IVentasHistoricaRepository, VentasHistoricaRepository>();

            //  REPOSITORIO DWH 
            builder.Services.AddScoped<IDwhRepository, DwhRepository>();

            //servicio 
            builder.Services.AddScoped<IVentasServices, VentasServices>();

           
            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}