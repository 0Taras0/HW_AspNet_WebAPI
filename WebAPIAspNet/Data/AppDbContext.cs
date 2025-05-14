using Microsoft.EntityFrameworkCore;
using WebAPIAspNet.Data.Entities;

namespace WebAPIAspNet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
