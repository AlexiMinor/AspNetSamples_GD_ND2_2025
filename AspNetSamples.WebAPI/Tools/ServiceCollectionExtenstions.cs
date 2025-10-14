using AspNetSamples.DataAccess.Handlers.CommandHandlers;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using AspNetSamples.Services;
using AspNetSamples.Services.Abstractions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
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
        services.AddScoped<ITokenService, TokenService>();
        
    }

    public static void RegisterLogger(this IServiceCollection services, IConfiguration configuration)
    {

        var options = new ConfigurationReaderOptions { SectionName = "Serilog" };
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration, options)
            .CreateLogger();
        services.AddSerilog(logger);
    }

    public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                var corsOrigins = configuration.GetSection("AppSettings:CorsOrigins").Get<string[]>(); 
                policy
                    .WithOrigins(corsOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
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

    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            };
        });
    }
}