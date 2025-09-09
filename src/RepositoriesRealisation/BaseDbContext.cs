using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.Models;
using System.Data.Common;
using System.Xml;

namespace RepositoriesRealisation
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AccountModel> AccountsReadOnly { get; set; } = null!;
        public DbSet<PlayerProfileModel> PlayerProfiles { get; set; } = null!;
        public DbSet<AccountModelProfileImage> AccountsProfileImages { get; set; } = null!;
        public DbSet<RewardDescriptionModel> RewardDescription { get; set; } = null!;
        public DbSet<RewardDescriptionModelIconImage> RewardDescriptionIconImages { get; set; } = null!;
        public DbSet<CompetitionLevelDataModel> CompetitionLevelModel { get; set; } = null!;
        public DbSet<CompetitionLevelDataModelData> CompetitionLevelModelData { get; set; } = null!;
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
            modelBuilder.Entity<AccountModel>(options =>
            {
                options.Ignore(x => x.PasswordHash); // the forbidden knowledge
            });
            ConnectOneToOne<AccountModel, PlayerProfileModel>(modelBuilder, nameof(AccountModel.Profile));
            ConnectOneToOne<CompetitionLevelDataModel, CompetitionLevelDataModelData>(modelBuilder, nameof(CompetitionLevelDataModel.LevelData));
            ConnectOneToOne<RewardDescriptionModel, RewardDescriptionModelIconImage>(modelBuilder, nameof(RewardDescriptionModel.IconImage));
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

        public async Task DoCreateAccount(AccountModel c)
        {
            try
            {
                await Database.ExecuteSqlAsync(
                    $"INSERT INTO account (login, username, email, password_hash, privilegy_level) VALUES ({c.Login}, {c.Login}, {c.Email}, {c.PasswordHash}, {c.AccountPrivilegyLevel})"
                    );
            }
            catch(Npgsql.PostgresException ex)
            {
                throw new FailedOperationException(ex.Message);
            }
        }
    }
}
