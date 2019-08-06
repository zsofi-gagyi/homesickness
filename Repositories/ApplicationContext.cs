using HomesicknessVisualiser.Models;
using Microsoft.EntityFrameworkCore;

namespace HomesicknessVisualiser.Repositories
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Record> Records { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}


