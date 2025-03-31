using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Core.RewardCondition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using RepositoriesRealisation.RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompetitiveBackend.Repositories
{
    internal class CompetitionRewardRepository : BaseRepository<CompetitionRewardRepository>, ICompetitionRewardRepository
    {
        public CompetitionRewardRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<CompetitionRewardRepository> logger) : base(contextFactory, logger)
        {
        }

        public async Task CreateCompetitionReward(CompetitionReward description)
        {
            using var context = await GetDbContext();
            if(!GrantConditionConverter.ToJSON(description.Condition, out JsonDocument d))
            {
                _logger.LogError($"There is no such condition supported: {description.Condition.Type}");
                throw new Exceptions.IncorrectOperationException($"There is no such condition supported: {description.Condition.Type}");
            }
            CompetitionRewardModel model = new CompetitionRewardModel(description.RewardDescriptionID, description.CompetitionID, d);
            try
            {
                await context.CompetitionReward.AddAsync(model);
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError("Could not add competition reward");
                throw new Exceptions.FailedOperationException("Could not add competition reward", ex);
            }
        }

        public async Task<CompetitionReward> GetCompetitionReward(int competitionRewardID)
        {
            using var context = await GetDbContext();
            CompetitionRewardModel model = await Find(context, competitionRewardID);
            return ToCore(model);
        }

        public async Task<IEnumerable<CompetitionReward>> GetCompetitionRewards(int competitionID)
        {
            using var context = await GetDbContext();
            try
            {
                var selectResult = await context.CompetitionReward.Where(x => x.CompetitionId == competitionID).Select(x => ToCore(x)).ToListAsync();
                return selectResult;
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not select data");
                throw new Exceptions.FailedOperationException($"Could not select data");
            }
        }


        public async Task RemoveCompetitionReward(int competitionRewardID)
        {
            using var context = await GetDbContext();
            CompetitionRewardModel model = await Find(context, competitionRewardID);
            try
            {
                context.CompetitionReward.Remove(model);
                await context.SaveChangesAsync();
            }
            catch(Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not remove competition reward with ID {competitionRewardID}");
                throw new Exceptions.FailedOperationException($"Could not remove competition reward with ID {competitionRewardID}");
            }
        }

        public async Task UpdateCompetitionReward(CompetitionReward description)
        {
            if (description.Id == null) throw new Exceptions.IncorrectOperationException("ID is null");
            if (!GrantConditionConverter.ToJSON(description.Condition, out JsonDocument d))
            {
                _logger.LogError($"There is no such condition supported: {description.Condition.Type}");
                throw new Exceptions.IncorrectOperationException($"There is no such condition supported: {description.Condition.Type}");
            }
            using var context = await GetDbContext();
            CompetitionRewardModel model = await Find(context, description.Id!.Value);
            model.RewardDescriptionId = description.RewardDescriptionID;
            model.Condition = d;
            model.CompetitionId = description.CompetitionID;
            try
            {
                context.CompetitionReward.Update(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex) when (ex.IsDBException())
            {
                _logger.LogError($"Could not update the reward");
                throw new Exceptions.FailedOperationException($"Could not update the reward");
            }
        }
        private CompetitionReward ToCore(CompetitionRewardModel model)
        {
            if (!GrantConditionConverter.FromJSON(model.Condition, out GrantCondition cond))
            {
                _logger.LogError($"Internal damage of GrantCondition field of reward {model.Id}");
                throw new Exceptions.FailedOperationException($"Internal damage of GrantCondition field of reward {model.Id}");
            }
            return new CompetitionReward(model.RewardDescriptionId,
                model.CompetitionId,
                model.RewardDescription.Name,
                model.RewardDescription.Description ?? string.Empty,
                cond,
                model.Id);
        }
        private async Task<CompetitionRewardModel> Find(BaseDbContext context, int id)
        {
            CompetitionRewardModel? model = await context.CompetitionReward.FindAsync(id);
            if (model == null)
            {
                _logger.LogError($"Could not find competition reward with ID {id}");
                throw new Exceptions.MissingDataException($"Could not find competition reward with ID {id}");
            }
            return model;
        }
    }
}
