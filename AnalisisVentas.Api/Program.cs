
using AnalisisVentas.Api.Data.Context;
using AnalisisVentas.Api.Data.Interface;
using AnalisisVentas.Api.Data.Repository;
using Microsoft.EntityFrameworkCore;



namespace AnalisisVentas.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddDbContext<ApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

          
            builder.Services.AddScoped<IApiClienteRepository, ApiClienteRepository>();
            builder.Services.AddScoped<IApiProductoRepository, ApiProductoRepository>();

            


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
