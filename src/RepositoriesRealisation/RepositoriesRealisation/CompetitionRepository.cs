using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    internal static class DateTimeExtensions
    {
        public static DateTime EnsureUTC(this DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();
        }
    }

    internal sealed class CompetitionRepository : BaseRepository<CompetitionRepository>, ICompetitionRepository
    {
        public CompetitionRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<CompetitionRepository> logger)
            : base(contextFactory, logger)
        {
        }

        public async Task<int> CreateCompetition(Competition c)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                EntityEntry<CompetitionModel> md = await context.Competition.AddAsync(new RepositoriesRealisation.Models.CompetitionModel(c.Name, c.Description, c.StartDate.EnsureUTC(), c.EndDate.EnsureUTC()));
                await context.SaveChangesAsync();
                return md.Entity.Id;
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError(ex, "Could not create Competition");
                throw new Exceptions.FailedOperationException("Could not create Competition", ex);
            }
        }

        public async Task<IEnumerable<Competition>> GetActiveCompetitions()
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                DateTime dt = DateTime.UtcNow;
                List<Competition> q = await context.Competition.Where(x => x.StartTime <= dt && dt <= x.EndTime)
                                                 .OrderByDescending(x => x.StartTime)
                                                 .Select(x => x.ToCoreModel())
                                                 .ToListAsync();
                return q;
            }
            catch (OperationCanceledException ex)
            {
                logger.LogError(ex, "Could not aquire active competitions");
                throw new Exceptions.FailedOperationException("Could not aquire active competitions", ex);
            }
        }

        public async Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                IQueryable<CompetitionModel> md = context.Competition.OrderByDescending((x) => x.StartTime);
                if (!dataLimiter.HasNoLimit)
                {
                    // _logger.LogInformation($"Skipping {dataLimiter.FirstIndex} and taking {dataLimiter.Partition}");
                    md = md.Skip(dataLimiter.FirstIndex).Take(dataLimiter.Partition);
                }

                List<Competition> models = await md.Select(x => x.ToCoreModel()).ToListAsync();
                return models;
            }
            catch (OperationCanceledException ex)
            {
                logger.LogError(ex, "Could not aquire competitions");
                throw new Exceptions.FailedOperationException("Could not aquire competitions", ex);
            }
        }

        public async Task<Competition> GetCompetition(int competitionID)
        {
            using BaseDbContext context = await GetDbContext();
            CompetitionModel? comp = await context.Competition.FindAsync(competitionID);
            if (comp == null)
            {
                logger.LogError($"Could not find competition with ID {competitionID}");
                throw new Exceptions.MissingDataException($"Could not find competition with ID {competitionID}");
            }

            return comp.ToCoreModel();
        }

        // public async Task<LargeData> GetCompetitionLevel(int competitionID)
        // {
        //    using BaseDbContext context = await GetDbContext();
        //    CompetitionLevelDataModelData? comp = await context.CompetitionLevelData.FindAsync(competitionID);
        //    if (comp == null)
        //    {
        //        _logger.LogError($"Could not find competition with ID {competitionID}");
        //        throw new Exceptions.MissingDataException($"Could not find competition with ID {competitionID}");
        //    }
        //    return new LargeData(comp.LevelData ?? Array.Empty<byte>());
        // }

        // public async Task SetCompetitionLevel(int competitionID, LargeData levelData)
        // {
        //    using BaseDbContext context = await GetDbContext();
        //    CompetitionLevelDataModelData? comp = await context.CompetitionLevelData.FindAsync(competitionID);
        //    if(comp == null)
        //    {
        //        _logger.LogError($"Could not find competition with ID {competitionID}");
        //        throw new Exceptions.MissingDataException($"Could not find competition with ID {competitionID}");
        //    }
        //    comp.LevelData = levelData.Data;
        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex) when (ex.IsDBException())
        //    {
        //        _logger.LogError("Could not update Competition");
        //        throw new Exceptions.FailedOperationException("Could not update Competition",ex);
        //    }
        // }
        public async Task UpdateCompetition(Competition c)
        {
            if (c.Id == null)
            {
                throw new Exceptions.IncorrectOperationException("Competition ID is null");
            }

            using BaseDbContext context = await GetDbContext();
            CompetitionModel? comp = await context.Competition.FindAsync(c.Id);
            if (comp == null)
            {
                logger.LogError($"Could not find competition with ID {c.Id}");
                throw new Exceptions.MissingDataException($"Could not find competition with ID {c.Id}");
            }

            comp.StartTime = c.StartDate.EnsureUTC();
            comp.EndTime = c.EndDate.EnsureUTC();
            comp.Description = c.Description;
            comp.Name = c.Name;
            try
            {
                context.Update(comp);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                logger.LogError("Could not update Competition");
                throw new Exceptions.FailedOperationException("Could not update Competition", ex);
            }
        }
    }
}
