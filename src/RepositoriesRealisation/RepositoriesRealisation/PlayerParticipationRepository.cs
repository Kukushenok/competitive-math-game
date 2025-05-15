using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore.Query;
namespace CompetitiveBackend.Repositories
{
    class PlayerParticipationRepository : BaseRepository<PlayerParticipationRepository>, IPlayerParticipationRepository
    {
        public PlayerParticipationRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<PlayerParticipationRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task CreateParticipation(PlayerParticipation participation)
        {
            using var context = await GetDbContext();
            try
            {
                await context.PlayerParticipation.AddAsync(new PlayerParticipationModel(participation.CompetitionId, participation.PlayerProfileId, participation.Score, participation.LastUpdateTime.EnsureUTC()));
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not create participation");
                throw new FailedOperationException("Could not create participation", ex);
            }
        }

        public async Task DeleteParticipation(int accountID, int competitionID)
        {
            using var context = await GetDbContext();
            try
            {
                int cnt = await context.PlayerParticipation.Where(x => x.AccountID == accountID && x.CompetitionID == competitionID).ExecuteDeleteAsync();
                if (cnt == 0)
                {
                    throw new MissingDataException("No data was deleted");
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not remove participation");
                throw new FailedOperationException("Could not remove participation", ex);
            }
        }

        public async Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter, bool bindPlayer = true, bool bindCompetition = false)
        {
            using var context = await GetDbContext();
            try
            {
                IQueryable<PlayerParticipationModel> md = context.PlayerParticipation.Where(x=>x.CompetitionID == competitionID).OrderByDescending((x) => x.Score);
                if(bindCompetition) md = md.Include(x => x.Competition);
                if (bindPlayer) md = md.Include(x => x.Account);
                if (!limiter.HasNoLimit)
                {
                    //_logger.LogInformation($"Skipping {dataLimiter.FirstIndex} and taking {dataLimiter.Partition}");
                    md = md.Skip(limiter.FirstIndex).Take(limiter.Partition);
                }
                var models = from x in (await md.ToListAsync()) select FromModel(x);
                return models;
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not fetch leaderboard");
                throw new FailedOperationException("Could not fetch leaderboard", ex);
            }
        }

        public async Task<PlayerParticipation> GetParticipation(int accountID, int competitionID, bool bindPlayer = false, bool bindCompetition = false)
        {
            using var context = await GetDbContext();
            IQueryable<PlayerParticipationModel> p = context.PlayerParticipation.AsQueryable();
            if (bindCompetition) p = p.Include(x => x.Competition);
            if (bindPlayer) p = p.Include(x => x.Account);
            PlayerParticipationModel? model = await p.FirstOrDefaultAsync(x => x.AccountID == accountID && x.CompetitionID == competitionID);
            if(model == null)
            {
                _logger.LogError($"Could not find participation of player {accountID} with competition {competitionID}");
                throw new MissingDataException($"Could not find participation of player {accountID} with competition {competitionID}");
            }
            return FromModel(model);
        }
        private PlayerParticipation FromModel(PlayerParticipationModel model)
        {
            return new PlayerParticipation(model.CompetitionID, model.AccountID, model.Score, model.LastUpdateTime, model.Account?.ToCoreModel(), model.Competition?.ToCoreModel());
        }

        public async Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int accountID, DataLimiter limiter, bool bindPlayer = false, bool bindCompetition = true)
        {
            using var context = await GetDbContext();
            try
            {
                IQueryable<PlayerParticipationModel> md = context.PlayerParticipation.Where(x => x.AccountID == accountID);
                if (bindCompetition) md = md.Include(x => x.Competition);
                if (bindPlayer) md = md.Include(x => x.Account);
                if (!limiter.HasNoLimit)
                {
                    //_logger.LogInformation($"Skipping {dataLimiter.FirstIndex} and taking {dataLimiter.Partition}");
                    md = md.Skip(limiter.FirstIndex).Take(limiter.Partition);
                }
                var models = from x in (await md.ToListAsync()) select FromModel(x);
                return models;
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not fetch leaderboard");
                throw new FailedOperationException("Could not fetch leaderboard", ex);
            }
        }

        public async Task UpdateParticipation(PlayerParticipation participation)
        {
            using var context = await GetDbContext();
            PlayerParticipationModel? model = await context.PlayerParticipation.FindAsync(participation.PlayerProfileId, participation.CompetitionId);
            if (model == null)
            {
                _logger.LogError($"Could not update participation of player {participation.PlayerProfileId} with competition {participation.CompetitionId}");
                throw new MissingDataException($"Could not update participation of player {participation.PlayerProfileId} with competition {participation.CompetitionId}");
            }
            model.Score = participation.Score;
            model.LastUpdateTime = participation.LastUpdateTime;
            try
            {
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Failed to update participation");
                throw new FailedOperationException("Failed to update participation", ex);
            }
        }
    }
}
