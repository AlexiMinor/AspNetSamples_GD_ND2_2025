using AspNetSamples.DataAccess.Handlers.CommandHandlers;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using AspNetSamples.Services;
using AspNetSamples.Services.Abstractions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Settings.Configuration;

namespace AspNetSamples.WebAPI.Tools;

public static class ServiceCollectionExtensions
{
    //generate method for registering services
    public static void RegisterServices(this IServiceCollection services)
    {
        //rewrite using reflection
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticleRateService, ArticleRateService>();
        services.AddScoped<IRssService, RssService>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWebParserService, WebParserService>();
        services.AddScoped<IHtmlCleanerService, HtmlCleanerService>();
        services.AddScoped<ISyndicationFeedReader, SyndicationFeedReader>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IMemoryCache, MemoryCache>();
        
    }

    public static void RegisterLogger(this IServiceCollection services, IConfiguration configuration)
    {

        var options = new ConfigurationReaderOptions { SectionName = "Serilog" };
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration, options)
            .CreateLogger();
        services.AddSerilog(logger);
    }

    public static void RegisterDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GoodArticleAggregatorContext>(
            opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<AddArticlesCollectionCommandHandler>());
    }
    
    public static void RegisterMappers(this IServiceCollection services)
    {
        services.AddScoped<ArticleMapper>();
        services.AddScoped<SourceMapper>();
        services.AddScoped<UserMapper>();
        services.AddScoped<RoleMapper>();
    }
    
    public static void RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer();

    }
}