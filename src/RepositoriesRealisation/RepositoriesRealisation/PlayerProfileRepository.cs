using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.DatabaseObjects;

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
            AccountModel? p = await context.Accounts.FindAsync(accountId);
            if (p == null) throw new Exceptions.MissingDataException();
            return p.ToCoreProfile();
        }

        public async Task<LargeData> GetPlayerProfileImage(int accountId)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? p = await context.Accounts.FindAsync(accountId);
            if (p == null) throw new Exceptions.MissingDataException();
            if (p.ProfileImage == null) return new LargeData(Array.Empty<byte>());
            return new LargeData(p.ProfileImage);
        }

        public async Task UpdatePlayerProfile(PlayerProfile profile)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? p = await context.Accounts.FindAsync(profile.Id);
            if (p == null) throw new Exceptions.MissingDataException();
            p.Description = profile.Description;
            p.Name = profile.Name;
            context.Accounts.Update(p);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePlayerProfileImage(int accountId, LargeData data)
        {
            using BaseDbContext context = await GetDbContext();
            AccountModel? p = await context.Accounts.FindAsync(accountId);
            if (p == null) throw new Exceptions.MissingDataException();
            p.ProfileImage = data.Data;
            context.Accounts.Update(p);
            await context.SaveChangesAsync();
        }
    }
}
