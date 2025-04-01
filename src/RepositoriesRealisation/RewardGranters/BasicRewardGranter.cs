using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RewardGranters
{
    internal class BasicRewardGranter : IRewardGranter
    {
        private ILogger<BasicRewardGranter> _logger;
        public BasicRewardGranter(ILogger<BasicRewardGranter> logger)
        {
            this._logger = logger;
        }

        public async Task GrantRewards(BaseDbContext context, int competitionID)
        {
            PlayerParticipationModel[] participations = await context.PlayerParticipation.Where(x => x.CompetitionID == competitionID)
                                                                                             .OrderByDescending(x=>x.Score)
                                                                                             .ToArrayAsync();
            List<CompetitionRewardModel> rewards = await context.CompetitionReward.Where(x => x.CompetitionId == competitionID)
                                                                                   .ToListAsync();
            foreach(CompetitionRewardModel model in rewards)
            {
                GrantCondition? cond = GrantConditionConverter.FromJSON(model.Condition);
                if(cond == null)
                {
                    _logger.LogWarning($"Processing {model.Id}: Could not read the reward condition; skipping");
                }
                else if(cond is RankGrantCondition rankCondition)
                {
                    await GrantByCondition(context, model, participations, rankCondition);
                }
                else if(cond is PlaceGrantCondition placeCondition)
                {
                    await GrantByCondition(context, model, participations, placeCondition);
                }
            }

            await context.SaveChangesAsync();
        }
        private async Task GrantByCondition(BaseDbContext context, CompetitionRewardModel model, PlayerParticipationModel[] participations, PlaceGrantCondition rankCondition)
        {
            if (participations.Length == 0)
                return;
            int competitionID = participations[0].CompetitionID;
            for (int i = rankCondition.minPlace - 1; i <= rankCondition.maxPlace && i < participations.Length; i++)
            {
                await context.PlayerReward.AddAsync(new PlayerRewardModel(participations[i].AccountID, model.RewardDescriptionId, competitionID));
            }
        }
        private async Task GrantByCondition(BaseDbContext context, CompetitionRewardModel model, PlayerParticipationModel[] participations, RankGrantCondition rankCondition)
        {
            if (participations.Length == 0)
                return;
            int minPlace = (int)Math.Floor(participations.Length * (1.0 - rankCondition.maxRank)) + 1;
            int maxPlace = (int)Math.Ceiling(participations.Length * (1.0 - rankCondition.minRank)) + 1;
            await GrantByCondition(context, model, participations, new PlaceGrantCondition(minPlace, maxPlace));
        }
    }
}
