using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Logging;
using Model;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    class CompetitionRepository : BaseRepository<CompetitionRepository>, ICompetitionRepository
    {
        public CompetitionRepository(IMongoConnectionCreator contextFactory, ILogger<CompetitionRepository> logger, IAutoIncrementManager manager) : base(contextFactory, logger, manager)
        {
        }

        public async Task<int> CreateCompetition(Competition c)
        {
            using var context = await GetMongoConnection();
            try
            {
                int id = await NewID(context);
                await context.Competition().InsertOneAsync(new Model.CompetitionEntity(id, c));
                await context.SaveChangesAsync();
                return id;
            }
            catch (Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Failed to create competition", ex);
            }
        }

        public async Task<IEnumerable<Competition>> GetActiveCompetitions()
        {
            using var context = await GetMongoConnection();
            try
            {
                var q = context.Competition().AsQueryable();
                DateTime dt = DateTime.UtcNow;
                var R = await q.Where(x => (x.StartTime <= dt && dt <= x.EndTime))
                                .OrderByDescending(x => x.StartTime)
                                .ToListAsync();
                return from r in R select r.Convert();
            }
            catch (Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Failed to aquire active competitions", ex);
            }
        }

        public async Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter)
        {
            using var context = await GetMongoConnection();
            try
            {
                IQueryable<CompetitionEntity> md = context.Competition().AsQueryable().OrderByDescending((x) => x.StartTime);
                if (!dataLimiter.HasNoLimit)
                {
                    md = md.Skip(dataLimiter.FirstIndex).Take(dataLimiter.Partition);
                }
                var models = await md.ToListAsync();
                return from r in models select r.Convert();
            }
            catch (Exception ex)
            {
                throw new CompetitiveBackend.Repositories.Exceptions.FailedOperationException("Failed to aquire competitions", ex);
            }
        }

        public async Task<Competition> GetCompetition(int competitionID)
        {
            using var context = await GetMongoConnection();
            try
            {
                return (await context.Competition().FindAsync(x => x.Id == competitionID)).Single().Convert();
            }
            catch(Exception ex)
            {
                _logger.LogError($"No competition with ID {competitionID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No competition with ID", ex);
            }
        }

        public async Task<LargeData> GetCompetitionLevel(int competitionID)
        {
            using var context = await GetMongoConnection();
            try
            {
                return new LargeData((await context.Competition().FindAsync(x => x.Id == competitionID)).Single().LevelData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"No competition with ID {competitionID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No competition with ID", ex);
            }
        }

        public async Task SetCompetitionLevel(int competitionID, LargeData levelData)
        {
            using var context = await GetMongoConnection();
            try
            {
                await context.Competition().UpdateOneAsync(Builders<CompetitionEntity>.Filter.Eq(x => x.Id, competitionID),
                    Builders<CompetitionEntity>.Update.Set(x => x.LevelData, levelData.Data));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"No competition with ID {competitionID}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No competition with ID", ex);
            }
        }

        public async Task UpdateCompetition(Competition c)
        {
            if (c.Id == null) throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("ID is null");
            using var context = await GetMongoConnection();
            try
            {
                await context.Competition().UpdateOneAsync(Builders<CompetitionEntity>.Filter.Eq(x => x.Id, c.Id),
                    Builders<CompetitionEntity>.Update.Combine(
                        Builders<CompetitionEntity>.Update.Set(x=>x.StartTime, c.StartDate),
                         Builders<CompetitionEntity>.Update.Set(x => x.EndTime, c.EndDate),
                          Builders<CompetitionEntity>.Update.Set(x => x.CompetitionName, c.Name),
                           Builders<CompetitionEntity>.Update.Set(x => x.Description, c.Description)
                        )
                    );
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"No competition with ID {c.Id}");
                throw new CompetitiveBackend.Repositories.Exceptions.MissingDataException("No competition with ID", ex);
            }
        }
    }
}
