using CompetitiveBackend.Core.Objects.Riddles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    internal sealed class RiddleSettingsRepository : BaseRepository<RiddleSettingsRepository>, IRiddleSettingsRepository
    {
        public RiddleSettingsRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<RiddleSettingsRepository> logger)
            : base(contextFactory, logger)
        {
        }

        public async Task<RiddleGameSettings> GetRiddleSettings(int competitionID)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                RiddleGameSettingsModel? settings = await context.RiddleGameSettings
                    .Where(s => s.Id == competitionID)
                    .FirstOrDefaultAsync();

                return settings == null
                    ? throw new ArgumentException($"Riddle settings for competition ID {competitionID} not found")
                    : settings.ToCoreModel();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not retrieve riddle settings for competition ID {CompetitionID}", competitionID);
                throw new Exceptions.FailedOperationException("Could not retrieve riddle settings", ex);
            }
        }

        public async Task UpdateRiddleSettings(int competitionID, RiddleGameSettings settings)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                RiddleGameSettingsModel? existingSettings = await context.RiddleGameSettings
                    .Where(s => s.Id == competitionID)
                    .FirstOrDefaultAsync();

                if (existingSettings != null)
                {
                    // Update existing settings
                    existingSettings.UpdateFromCoreModel(settings);
                    context.RiddleGameSettings.Update(existingSettings);
                }
                else
                {
                    throw new Exceptions.MissingDataException("No such data");
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not update riddle settings for competition ID {CompetitionID}", competitionID);
                throw new Exceptions.FailedOperationException("Could not update riddle settings", ex);
            }
        }
    }
}
