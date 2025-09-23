using AspNetSamples.Database.Entities;
using AspNetSamples.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyCustomUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Article> Articles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
