using AnalisisVentas.Application.Repositories;
using AnalisisVentas.Application.Repositories.IApiRepository;
using AnalisisVentas.Persistencia.Repositories.Api;

namespace AnalisisVentas.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                // Lee la URL 
                client.BaseAddress = new Uri(builder.Configuration["ApiSources:BaseUrl"]);
            });

          
         
            builder.Services.AddScoped<IClienteApiRepository, ClienteApiRepository>();
            builder.Services.AddScoped<IProductApiRepositoriy, ProductoApiRepository>();

         

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
