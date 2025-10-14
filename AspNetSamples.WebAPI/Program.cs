using System.Reflection;
using AspNetSamples.WebAPI.Filters;
using AspNetSamples.WebAPI.Tools;
using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using Bearer scheme. \r\n\r\nEnter 'Bearer'[space] and then your token in the text input below",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.RegisterServices();
builder.Services.RegisterMappers();
builder.Services.RegisterLogger(builder.Configuration);
builder.Services.RegisterDb(builder.Configuration);
builder.Services.RegisterHangfire(builder.Configuration);
builder.Services.AddResponseCaching();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(/*opt =>
    {
        //opt.SwaggerEndpoint();
    }*/);
}
app.UseForwardedHeaders(new ForwardedHeadersOptions()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
}); 
app.UseHttpsRedirection();
app.UseCors();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard(options: new DashboardOptions()
{
    Authorization = [new HangfireAuthorizationFilter()]
});
app.MapControllers();

app.Run();
