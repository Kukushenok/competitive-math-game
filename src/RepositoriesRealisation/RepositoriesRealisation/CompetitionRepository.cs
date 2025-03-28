using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Repositories
{
    public class CompetitionRepository : BaseRepository<CompetitionRepository>, ICompetitionRepository
    {
        public CompetitionRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<CompetitionRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task CreateCompetition(Competition c)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                await context.Competition.AddAsync(new RepositoriesRealisation.Models.CompetitionModel(c.Name, c.Description, c.StartDate, c.EndDate));
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is OperationCanceledException ||
                                       ex is DbUpdateException ||
                                       ex is DbUpdateConcurrencyException)
            {
                _logger.LogError(ex, "Could not create Competition");
                throw new Exceptions.FailedOperationException("Could not create Competition",ex);
            }

        }

        public async Task<IEnumerable<Competition>> GetActiveCompetitions()
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                DateTime dt = DateTime.Now;
                var q = await context.Competition.Where(x=>x.IsActive(dt))
                                                 .Select(x=>x.ToCoreModel())
                                                 .ToListAsync();
                return q;
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogError(ex, "Could not aquire active competitions");
                throw new Exceptions.FailedOperationException("Could not aquire active competitions", ex);
            }
        }

        public async Task<IEnumerable<Competition>> GetAllCompetitions(DataLimiter dataLimiter)
        {
            using BaseDbContext context = await GetDbContext();
            try
            {
                var models = await context.Competition.OrderBy((x)=>x.StartTime)
                                                      .Take(dataLimiter.ToRange())
                                                      .Select(x=>x.ToCoreModel())
                                                      .ToListAsync();
                return models;
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogError(ex, "Could not aquire competitions");
                throw new Exceptions.FailedOperationException("Could not aquire competitions", ex);
            }
        }

        public async Task<Competition> GetCompetition(int competitionID)
        {
            using BaseDbContext context = await GetDbContext();
            CompetitionModel? comp = await context.Competition.FindAsync(competitionID);
            if (comp == null)
            {
                _logger.LogError($"Could not find competition with ID {competitionID}");
                throw new Exceptions.MissingDataException($"Could not find competition with ID {competitionID}");
            }
            return comp.ToCoreModel();
        }

        public async Task<LargeData> GetCompetitionLevel(int competitionID)
        {
            using BaseDbContext context = await GetDbContext();
            CompetitionModel? comp = await context.Competition.FindAsync(competitionID);
            if (comp == null)
            {
                _logger.LogError($"Could not find competition with ID {competitionID}");
                throw new Exceptions.MissingDataException($"Could not find competition with ID {competitionID}");
            }
            return new LargeData(comp.LevelData);
        }
        
        public async Task SetCompetitionLevel(int competitionID, LargeData levelData)
        {
            using BaseDbContext context = await GetDbContext();
            CompetitionModel? comp = await context.Competition.FindAsync(competitionID);
            if(comp == null)
            {
                _logger.LogError($"Could not find competition with ID {competitionID}");
                throw new Exceptions.MissingDataException($"Could not find competition with ID {competitionID}");
            }
            comp.LevelData = levelData.Data;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is OperationCanceledException ||
                                       ex is DbUpdateException ||
                                       ex is DbUpdateConcurrencyException)
            {
                _logger.LogError("Could not update Competition");
                throw new Exceptions.FailedOperationException("Could not update Competition",ex);
            }
        }

        public async Task UpdateCompetition(Competition c)
        {
            if (c.Id == null) 
                throw new Exceptions.IncorrectOperationException("Competition ID is null");
            using BaseDbContext context = await GetDbContext();
            CompetitionModel? comp = await context.Competition.FindAsync(c.Id);
            if (comp == null)
            {
                _logger.LogError($"Could not find competition with ID {c.Id}");
                throw new Exceptions.MissingDataException($"Could not find competition with ID {c.Id}");
            }
            comp.StartTime = c.StartDate;
            comp.EndTime = c.EndDate;
            comp.Description = c.Description;
            comp.Name = c.Name;
            try
            {
                context.Update(comp);
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex is OperationCanceledException ||
                                       ex is DbUpdateException ||
                                       ex is DbUpdateConcurrencyException)
            {
                _logger.LogError("Could not update Competition");
                throw new Exceptions.FailedOperationException("Could not update Competition", ex);
            }
        }
    }
}
