using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitiveBackend.Repositories.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoriesRealisation.RepositoriesRealisation
{
    internal class CompetitionLevelDataRepository : BaseRepository<CompetitionLevelDataRepository>, ICompetitionLevelRepository
    {
        public CompetitionLevelDataRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<CompetitionLevelDataRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task AddCompetitionLevel(LargeData data, LevelDataInfo levelData)
        {
            using var context = await GetDbContext();
            try
            {
                CompetitionLevelDataModelData dat = new CompetitionLevelDataModelData();
                dat.LevelData = data.Data;
                CompetitionLevelDataModel model = new CompetitionLevelDataModel(levelData);
                model.LevelData = dat;
                context.CompetitionLevelModel.Add(model);
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Cannot create competition level data: {ex.Message}");
                throw new FailedOperationException("Could not create competition level data", ex);
            }
        }

        public async Task DeleteCompetitionLevel(int levelId)
        {
            using var context = await GetDbContext();
            try
            {
                int count = await context.CompetitionLevelModel.Where(x => x.Id == levelId).ExecuteDeleteAsync();
                if (count == 0) throw new MissingDataException($"There is no level with ID {levelId}");
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not delete level {levelId}");
                throw new FailedOperationException($"Could not delete level {levelId}", ex);
            }
        }

        private LevelDataInfo ToCore(CompetitionLevelDataModel model)
        {
            return new LevelDataInfo(model.CompetitionID, model.VersionKey, model.Platform, model.Id);
        }
        public async Task<IEnumerable<LevelDataInfo>> GetAllLevelData(int competitionID)
        {
            using var context = await GetDbContext();
            try
            {
                var result = await context.CompetitionLevelModel.Where(x => x.CompetitionID == competitionID).Select(x=>ToCore(x)).ToListAsync();
                return result;
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not get levels of {competitionID}");
                throw new FailedOperationException($"Could not get levels of {competitionID}", ex);
            }
        }

        public async Task<LargeData> GetCompetitionLevel(int competitionID, string? Platform = null, int? MaxVersion = null)
        {
            using var context = await GetDbContext();
            try
            {
                IQueryable<CompetitionLevelDataModel> models = context.CompetitionLevelModel;
                if (Platform != null)
                {
                    models = models.Where(x => x.Platform == Platform);
                }
                if(MaxVersion != null)
                {
                    models = models.Where(x => x.VersionKey <= MaxVersion);
                }
                var result = models.Select(x => ToCore(x)).MaxBy(x=>x.VersionCode);
                var levelDataHolder = await context.CompetitionLevelModelData.FindAsync(result?.Id);
                return new LargeData(levelDataHolder?.LevelData ?? []);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not get competition level of {competitionID}");
                throw new FailedOperationException($"Could not get level of {competitionID}", ex);
            }
        }

        public async Task<LargeData> GetCompetitionLevel(int levelDataID)
        {
            using var context = await GetDbContext();
            try
            {
                var levelDataHolder = await context.CompetitionLevelModelData.FindAsync(levelDataID);
                if (levelDataHolder == null)
                {
                    _logger.LogError($"No data presented for {levelDataID}");
                    throw new MissingDataException($"No data presented for {levelDataID}");
                }
                return new LargeData(levelDataHolder.LevelData!);
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not get competition level of {levelDataID}");
                throw new FailedOperationException($"Could not get level of {levelDataID}", ex);
            }
        }

        public async Task UpdateCompetitionLevelData(int levelDataID, LargeData data)
        {
            using var context = await GetDbContext();
            var rd = await context.CompetitionLevelModelData.FindAsync(levelDataID);
            if(rd == null)
            {
                _logger.LogError($"Could not get competition level of {levelDataID}");
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
                _logger.LogError(exception, $"Could not update reward icon image for {levelDataID}");
                throw new FailedOperationException();
            }
        }

        public async Task UpdateCompetitionLevelInfo(LevelDataInfo levelData)
        {
            if (levelData.Id == null) throw new IncorrectOperationException("levelData.ID cannot be null");
            using var context = await GetDbContext();
            var rd = await context.CompetitionLevelModel.FindAsync(levelData.Id);
            if (rd == null)
            {
                _logger.LogError($"Could not get competition level of {levelData.Id}");
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
                _logger.LogError(exception, $"Could not update reward icon image for {levelData.Id}");
                throw new FailedOperationException();
            }
        }
    }
}
