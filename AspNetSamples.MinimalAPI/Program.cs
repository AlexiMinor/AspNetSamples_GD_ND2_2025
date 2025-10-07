using AspNetSamples.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GoodArticleAggregatorContext>(opt 
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

app.MapGet("/api/articles", async (GoodArticleAggregatorContext db) =>
{
    var articles = await db.Articles.ToListAsync();
    return articles;
});

app.Run();
