using AspNetSamples.Database;
using AspNetSamples.Services;
using AspNetSamples.Services.Abstractions;
using AspNetSamples.UI.Configuration;
using AspNetSamples.UI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetSamples.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configuration
            // 0. Chained configuration provider -> object to read configuration from all sources
            // 1. appsettings.json
            // 2. appsettings.{Environment}.json
            // If environment is Development -> App secrets
            // 3. Environment variables
            // 4. Command line arguments
            // 5. User configuration files or other sources

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddJsonFile("secrets.json");

            builder.Services.Configure<PageConfigInfo>(builder.Configuration.GetSection("AppSettings:PageConfigInfo"));

            // Add services to the container - DI, IoC Container.
            // Connect to DB
            // Mappers
            builder.Services.AddControllersWithViews();

            ////builder.Services.AddMvcCore();
            //builder.Services.AddControllers();

            // Register the DbContext with the DI container
            builder.Services.AddDbContext<GoodArticleAggregatorContext>(
                opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //register services
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ISourceService, SourceService>();
            builder.Services.AddTransient<ILifeTimeSampleService, LifetimeService>();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();
            //builder.Services.AddSingleton(typeof(ISingletonService), typeof(SingletonService));

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

            app.UseMiddleware<TestMiddleware>();

            //app.Map("/api/test",
            //    () => ((IArticleService)app.Services.GetService(typeof(IArticleService))).GetArticlesByPageAsync(15)
            //        .Result);

            
            app.Map("/some/path",
                (IOptions<PageConfigInfo> options) => options.Value.DefaultPageSize);

            app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) 
                => string.Join(Environment.NewLine, endpointSources.SelectMany(source => source.Endpoints)));
            
            app.MapDefaultControllerRoute();
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.MapControllerRoute(
            //    name: "secondary",
            //    pattern: "hidden/{action=Index}/{controller=Home}/{name?}/{id?}");
            //app.MapFallbackToController("Page404","Home");
            app.Run();
        }
    }
}
