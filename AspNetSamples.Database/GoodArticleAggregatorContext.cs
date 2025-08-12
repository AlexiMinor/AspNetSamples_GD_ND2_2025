using AspNetSamples.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Database
{
    public class GoodArticleAggregatorContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Source> Sources { get; set; }
        
        
        public GoodArticleAggregatorContext (DbContextOptions<GoodArticleAggregatorContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Configure the context to use SQL Server with a connection string from config
        //    optionsBuilder
        //        .UseSqlServer("Data Source=localhost;Initial Catalog=GoodArticleAggregator;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");
        //}
    }
}
