using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    internal class PlayerProfileRepository : BaseRepository<PlayerProfileRepository>, IPlayerProfileRepository
    {
        public PlayerProfileRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<PlayerProfileRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task<PlayerProfile> GetPlayerProfile(int accountId)
        {
            using BaseDbContext context = await GetDbContext();
            PlayerProfileModel? p = await context.PlayerProfiles.FindAsync(accountId);
            if (p == null) throw new Exceptions.MissingDataException();
            return p.ToCoreModel();
        }

        public async Task<LargeData> GetPlayerProfileImage(int accountId)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModelProfileImage? p = await context.AccountsProfileImages.FindAsync(accountId);
            if (p == null) throw new Exceptions.MissingDataException();
            if (p.ProfileImage == null) return new LargeData(Array.Empty<byte>());
            return new LargeData(p.ProfileImage);
        }

        public async Task UpdatePlayerProfile(PlayerProfile profile)
        {
            using BaseDbContext context = await GetDbContext();
            PlayerProfileModel? p = await context.PlayerProfiles.FindAsync(profile.Id);
            if (p == null) throw new Exceptions.MissingDataException();
            p.Description = profile.Description;
            p.Name = profile.Name;
            try
            {
                context.PlayerProfiles.Update(p);
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not update player profile");
                throw new Exceptions.FailedOperationException("Could not update player profile");
            }
        }

        public async Task UpdatePlayerProfileImage(int accountId, LargeData data)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModelProfileImage? p = await context.AccountsProfileImages.FindAsync(accountId);
            if (p == null) throw new Exceptions.MissingDataException();
            p.ProfileImage = data.Data;
            try
            {
                context.AccountsProfileImages.Update(p);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not update player profile picture");
                throw new Exceptions.FailedOperationException("Could not update player profile picture");
            }
        }
    }
}
