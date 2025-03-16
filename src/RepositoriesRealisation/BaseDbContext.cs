using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoriesRealisation.DatabaseObjects;

namespace RepositoriesRealisation
{
    public class BaseDbContext: DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AccountModel> Accounts { get; set; }
        protected BaseDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
