using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.Models;

namespace RepositoriesRealisation
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<RewardDescriptionModel> RewardDescription { get; set; }
        public DbSet<CompetitionModel> Competition { get; set; }
        protected BaseDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
