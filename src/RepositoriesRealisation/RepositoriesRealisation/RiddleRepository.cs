using System.Text.Json;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.RepositoriesRealisation;
namespace CompetitiveBackend.Repositories
{
    internal sealed class RiddleRepository : BaseRepository<RiddleRepository>, IRiddleRepository
    {
        public RiddleRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<RiddleRepository> logger)
            : base(contextFactory, logger)
        {
        }

        public async Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter)
        {
            await using BaseDbContext context = await GetDbContext();
            try
            {
                IQueryable<CompetitionRiddleModel> query = context.CompetitionRiddles
                    .AsNoTracking()
                    .Where(r => r.CompetitionID == competitionID);

                if (!limiter.HasNoLimit)
                {
                    query = query.Skip(limiter.FirstIndex).Take(limiter.Partition);
                }

                List<CompetitionRiddleModel> models = await query.ToListAsync();
                return models.Select(m => m.ToDomain());
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Failed to retrieve riddles for competition {CompetitionID}", competitionID);
                throw new Exceptions.FailedOperationException($"Could not retrieve riddles for competition {competitionID}", ex);
            }
        }

        public async Task CreateRiddle(RiddleInfo riddle)
        {
            await using BaseDbContext context = await GetDbContext();
            try
            {
                var model = CompetitionRiddleModel.FromDomain(riddle);
                context.CompetitionRiddles.Add(model);

                await context.SaveChangesAsync();
                logger.LogInformation("Created riddle {RiddleID}", model.ID);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not create riddle for competition {CompetitionID}", riddle.CompetitionID);
                throw new Exceptions.FailedOperationException("Could not create riddle", ex);
            }
        }

        public async Task UpdateRiddle(RiddleInfo riddle)
        {
            await using BaseDbContext context = await GetDbContext();
            try
            {
                CompetitionRiddleModel? model = await context.CompetitionRiddles
                    .FirstOrDefaultAsync(x => x.ID == riddle.Id);

                if (model == null)
                {
                    logger.LogWarning("Attempted to update non-existent riddle ID {RiddleID}", riddle.Id);
                    throw new Exceptions.MissingDataException($"Riddle with ID {riddle.Id} not found");
                }

                model.Question = riddle.Question;
                model.Answer = riddle.TrueAnswer.TextAnswer;
                model.OtherAnswers = JsonSerializer.Serialize(
                    riddle.PossibleAnswers);

                context.CompetitionRiddles.Update(model);
                await context.SaveChangesAsync();

                logger.LogInformation("Updated riddle {RiddleID}", riddle.Id);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not update riddle ID {RiddleID}", riddle.Id);
                throw new Exceptions.FailedOperationException($"Could not update riddle {riddle.Id}", ex);
            }
        }

        public async Task DeleteRiddle(int riddleID)
        {
            await using BaseDbContext context = await GetDbContext();
            try
            {
                CompetitionRiddleModel? model = await context.CompetitionRiddles.FirstOrDefaultAsync(x => x.ID == riddleID);
                if (model == null)
                {
                    logger.LogWarning("Attempted to delete non-existent riddle ID {RiddleID}", riddleID);
                    throw new Exceptions.FailedOperationException($"Riddle with ID {riddleID} not found");
                }

                context.CompetitionRiddles.Remove(model);
                await context.SaveChangesAsync();

                logger.LogInformation("Deleted riddle {RiddleID}", riddleID);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not delete riddle ID {RiddleID}", riddleID);
                throw new Exceptions.FailedOperationException($"Could not delete riddle {riddleID}", ex);
            }
        }
    }
}
