using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation.Models;

namespace RepositoriesRealisation.RepositoriesRealisation
{
    [Obsolete("Disfunct")]
    internal sealed class CompetitionLevelDataRepository : BaseRepository<CompetitionLevelDataRepository>, ICompetitionLevelRepository
    {
        public CompetitionLevelDataRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<CompetitionLevelDataRepository> logger)
            : base(contextFactory, logger)
        {
        }

        [Obsolete]
        public async Task AddCompetitionLevel(LargeData data, LevelDataInfo levelData)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                var dat = new CompetitionLevelDataModelData
                {
                    LevelData = data.Data,
                };
                var model = new CompetitionLevelDataModel(levelData)
                {
                    LevelData = dat,
                };
                context.CompetitionLevelModel.Add(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Cannot create competition level data: {ex.Message}");
                throw new FailedOperationException("Could not create competition level data", ex);
            }
        }

        public async Task DeleteCompetitionLevel(int levelId)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                CompetitionLevelDataModel? model = await context.CompetitionLevelModel.FindAsync(levelId) ?? throw new MissingDataException($"There is no level with ID {levelId}");
                context.CompetitionLevelModel.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Could not delete level {levelId}");
                throw new FailedOperationException($"Could not delete level {levelId}", ex);
            }
        }

        [Obsolete]
        private LevelDataInfo ToCore(CompetitionLevelDataModel model)
        {
            return new LevelDataInfo(model.CompetitionID, model.VersionKey, model.Platform, model.Id);
        }

        [Obsolete]
        public async Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionID)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                List<CompetitionLevelDataModel> result = await context.CompetitionLevelModel.Where(x => x.CompetitionID == competitionID).ToListAsync();
                return from r in result select ToCore(r);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Could not get levels of {competitionID}");
                throw new FailedOperationException($"Could not get levels of {competitionID}", ex);
            }
        }

        public async Task<LargeData> GetCompetitionLevel(int competitionID, string? platform = null, int? maxVersion = null)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                IQueryable<CompetitionLevelDataModel> models = context.CompetitionLevelModel.Where(x => x.CompetitionID == competitionID);
                if (platform != null)
                {
                    models = models.Where(x => x.Platform == platform);
                }

                if (maxVersion != null)
                {
                    models = models.Where(x => x.VersionKey <= maxVersion);
                }

                CompetitionLevelDataModel? result = (await models.ToListAsync()).MaxBy(x => x.VersionKey);
                CompetitionLevelDataModelData? levelDataHolder = await context.CompetitionLevelModelData.FindAsync(result?.Id);
                return new LargeData(levelDataHolder?.LevelData ?? []);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Could not get competition level of {competitionID}");
                throw new FailedOperationException($"Could not get level of {competitionID}", ex);
            }
        }

        public async Task<LargeData> GetSpecificCompetitionLevel(int levelDataID)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                CompetitionLevelDataModelData? levelDataHolder = await context.CompetitionLevelModelData.FindAsync(levelDataID);
                if (levelDataHolder == null)
                {
                    logger.LogError($"No data presented for {levelDataID}");
                    throw new MissingDataException($"No data presented for {levelDataID}");
                }

                return new LargeData(levelDataHolder.LevelData!);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Could not get competition level of {levelDataID}");
                throw new FailedOperationException($"Could not get level of {levelDataID}", ex);
            }
        }

        [Obsolete]
        public async Task<LevelDataInfo> GetSpecificCompetitionLevelInfo(int levelDataID)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                CompetitionLevelDataModel? levelDataHolder = await context.CompetitionLevelModel.FindAsync(levelDataID);
                if (levelDataHolder == null)
                {
                    logger.LogError($"No data presented for {levelDataID}");
                    throw new MissingDataException($"No data presented for {levelDataID}");
                }

                return ToCore(levelDataHolder);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError($"Could not get competition level of {levelDataID}");
                throw new FailedOperationException($"Could not get level of {levelDataID}", ex);
            }
        }

        public async Task UpdateCompetitionLevelData(int levelDataID, LargeData data)
        {
            using BaseDbContext context = await GetDbContext();
            CompetitionLevelDataModelData? rd = await context.CompetitionLevelModelData.FindAsync(levelDataID);
            if (rd == null)
            {
                logger.LogError($"Could not get competition level of {levelDataID}");
                throw new FailedOperationException($"Could not get level of {levelDataID}");
            }

            rd.LevelData = data.Data;
            try
            {
                context.CompetitionLevelModelData.Update(rd);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                logger.LogError(exception, $"Could not update reward icon image for {levelDataID}");
                throw new FailedOperationException();
            }
        }

        [Obsolete]
        public async Task UpdateCompetitionLevelInfo(LevelDataInfo levelData)
        {
            if (levelData.Id == null)
            {
                throw new IncorrectOperationException("levelData.ID cannot be null");
            }

            using BaseDbContext context = await GetDbContext();
            CompetitionLevelDataModel? rd = await context.CompetitionLevelModel.FindAsync(levelData.Id);
            if (rd == null)
            {
                logger.LogError($"Could not get competition level of {levelData.Id}");
                throw new FailedOperationException($"Could not get level of {levelData.Id}");
            }

            rd.Platform = levelData.PlatformName;
            rd.VersionKey = levelData.VersionCode;
            rd.CompetitionID = levelData.CompetitionID;
            try
            {
                context.CompetitionLevelModel.Update(rd);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                logger.LogError(exception, $"Could not update reward icon image for {levelData.Id}");
                throw new FailedOperationException();
            }
        }
    }
}
