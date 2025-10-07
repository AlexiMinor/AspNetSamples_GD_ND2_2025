using System.Reflection;
using AspNetSamples.WebAPI.Tools;
using Hangfire;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    //generate info for OpenAPI
    //opt.SwaggerDoc("v0.1", new OpenApiInfo()
    //{
    //    Version = "v0.1",
    //    Title = "",
        
    //});

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.RegisterServices();
builder.Services.RegisterMappers();
builder.Services.RegisterLogger(builder.Configuration);
builder.Services.RegisterDb(builder.Configuration);
builder.Services.RegisterHangfire(builder.Configuration);
builder.Services.AddResponseCaching();


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

app.UseHttpsRedirection();
app.UseHangfireDashboard();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
