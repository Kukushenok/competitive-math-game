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
        public DbSet<AccountModel> Accounts { get; set; } = null!;
        public DbSet<RewardDescriptionModel> RewardDescription { get; set; } = null!;
        public DbSet<CompetitionModel> Competition { get; set; } = null!;
        public DbSet<PlayerParticipationModel> PlayerParticipation { get; set; } = null!;
        public DbSet<PlayerRewardModel> PlayerReward { get; set; } = null!;
        public DbSet<CompetitionRewardModel> CompetitionReward { get; set; } = null!;
        protected BaseDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerParticipationModel>().HasKey(table => new { table.AccountID, table.CompetitionID });
            base.OnModelCreating(modelBuilder);
        }
    }
}
