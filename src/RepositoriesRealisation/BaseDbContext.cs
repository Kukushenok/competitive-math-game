using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.Models;

namespace RepositoriesRealisation
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<AccountModel> AccountsReadOnly { get; set; } = null!;
        public DbSet<PlayerProfileModel> PlayerProfiles { get; set; } = null!;
        public DbSet<AccountModelProfileImage> AccountsProfileImages { get; set; } = null!;
        public DbSet<RewardDescriptionModel> RewardDescription { get; set; } = null!;
        public DbSet<RewardDescriptionModelIconImage> RewardDescriptionIconImages { get; set; } = null!;
        [Obsolete("Defunct")]
        public DbSet<CompetitionLevelDataModel> CompetitionLevelModel { get; set; } = null!;
        [Obsolete("Defunct")]
        public DbSet<CompetitionLevelDataModelData> CompetitionLevelModelData { get; set; } = null!;
        public DbSet<CompetitionModel> Competition { get; set; } = null!;
        public DbSet<PlayerParticipationModel> PlayerParticipation { get; set; } = null!;
        public DbSet<PlayerRewardModel> PlayerReward { get; set; } = null!;
        public DbSet<CompetitionRewardModel> CompetitionReward { get; set; } = null!;
        public DbSet<CompetitionRiddleModel> CompetitionRiddles { get; set; } = null!;
        public DbSet<RiddleGameSettingsModel> RiddleGameSettings { get; set; } = null!;
        protected BaseDbContext()
        {
        }

        private static void AddCompetitionModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompetitionRiddleModel>(entity =>
            {
                entity.ToTable("competition_riddle");

                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).HasColumnName("id");

                entity.HasOne(e => e.Competition)
                    .WithMany()
                    .HasForeignKey(e => e.CompetitionID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Question).HasMaxLength(256);
                entity.Property(e => e.Answer).HasMaxLength(256);
                entity.Property(e => e.OtherAnswers).HasColumnType("jsonb");
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerParticipationModel>().HasKey(table => new { table.AccountID, table.CompetitionID });
            modelBuilder.Entity<AccountModel>(options =>
            {
                options.Ignore(x => x.PasswordHash); // the forbidden knowledge
            });
            ConnectOneToOne<AccountModel, PlayerProfileModel>(modelBuilder, nameof(AccountModel.Profile));
            AddCompetitionModel(modelBuilder);

            // ConnectOneToOne<CompetitionLevelDataModel, CompetitionLevelDataModelData>(modelBuilder, nameof(CompetitionLevelDataModel.LevelData));
            ConnectOneToOne<RewardDescriptionModel, RewardDescriptionModelIconImage>(modelBuilder, nameof(RewardDescriptionModel.IconImage));
            ConnectOneToOne<AccountModel, AccountModelProfileImage>(modelBuilder, nameof(AccountModel.ProfileImage));
            ConnectOneToOne<CompetitionModel, RiddleGameSettingsModel>(modelBuilder, nameof(CompetitionModel.RiddleGameSettings));
            base.OnModelCreating(modelBuilder);
        }

        private static void ConnectOneToOne<T, TOther>(ModelBuilder builder, string propertyName)
            where TOther : OneToOneEntity<T>
            where T : class
        {
            builder.Entity<T>()
                .HasOne(typeof(TOther), propertyName)
                .WithOne(nameof(OneToOneEntity<T>.Model))
                .HasForeignKey(typeof(TOther), "Id");
        }

        public async Task DoCreateAccount(AccountModel c)
        {
            try
            {
                await Database.ExecuteSqlAsync(
                    $"INSERT INTO account (login, username, email, password_hash, privilegy_level) VALUES ({c.Login}, {c.Login}, {c.Email}, {c.PasswordHash}, {c.AccountPrivilegyLevel})");
            }
            catch (Npgsql.PostgresException ex)
            {
                throw new FailedOperationException(ex.Message);
            }
        }
    }
}
