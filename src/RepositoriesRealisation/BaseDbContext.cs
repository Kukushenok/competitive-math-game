using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.Models;
using System.Xml;

namespace RepositoriesRealisation
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AccountModel> Accounts { get; set; } = null!;
        public DbSet<PlayerProfileModel> PlayerProfiles { get; set; } = null!;
        public DbSet<AccountModelProfileImage> AccountsProfileImages { get; set; } = null!;
        public DbSet<RewardDescriptionModel> RewardDescription { get; set; } = null!;
        public DbSet<RewardDescriptionModelIconImage> RewardDescriptionIconImages { get; set; } = null!;
        public DbSet<RewardDescriptionModelInGameData> RewardDescriptionInGameData { get; set; } = null!;
        public DbSet<CompetitionModel> Competition { get; set; } = null!;
        public DbSet<CompetitionModelLevelData> CompetitionLevelData { get; set; } = null!;
        public DbSet<PlayerParticipationModel> PlayerParticipation { get; set; } = null!;
        public DbSet<PlayerRewardModel> PlayerReward { get; set; } = null!;
        public DbSet<CompetitionRewardModel> CompetitionReward { get; set; } = null!;
        protected BaseDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerParticipationModel>().HasKey(table => new { table.AccountID, table.CompetitionID });
            ConnectOneToOne<AccountModel, PlayerProfileModel>(modelBuilder, nameof(AccountModel.Profile));
            ConnectOneToOne<CompetitionModel, CompetitionModelLevelData>(modelBuilder, nameof(CompetitionModel.LevelData));
            ConnectOneToOne<RewardDescriptionModel, RewardDescriptionModelIconImage>(modelBuilder, nameof(RewardDescriptionModel.IconImage));
            ConnectOneToOne<RewardDescriptionModel, RewardDescriptionModelInGameData>(modelBuilder, nameof(RewardDescriptionModel.InGameData));
            ConnectOneToOne<AccountModel, AccountModelProfileImage>(modelBuilder, nameof(AccountModel.ProfileImage));
            base.OnModelCreating(modelBuilder);
        }
        private void ConnectOneToOne<T, Q>(ModelBuilder builder, string propertyName) where Q: OneToOneEntity<T>
                                                                 where T : class
        {
            builder.Entity<T>()
                .HasOne(typeof(Q), propertyName)
                .WithOne(nameof(OneToOneEntity<T>.Model))
                .HasForeignKey(typeof(Q), "Id");
        }
    }
}
