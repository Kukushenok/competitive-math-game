using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation.Models;

namespace RepositoriesRealisation.RewardGranters
{
    internal sealed class BasicRewardGranter : IRewardGranter
    {
        private readonly ILogger<BasicRewardGranter> logger;
        public BasicRewardGranter(ILogger<BasicRewardGranter> logger)
        {
            this.logger = logger;
        }

        public async Task GrantRewards(BaseDbContext context, int competitionID)
        {
            CompetitionModel? competition = await context.Competition.FindAsync(competitionID);
            if (competition == null)
            {
                throw new MissingDataException($"No competition with ID {competitionID}");
            }
            else if (competition.HasEnded)
            {
                throw new FailedOperationException($"Rewards have already been granted for competition {competitionID}");
            }

            context.Competition.Where(x => x.Id == competitionID).ExecuteUpdate(x => x.SetProperty(x => x.HasEnded, true));
            PlayerParticipationModel[] participations = await context.PlayerParticipation.Where(x => x.CompetitionID == competitionID)
                                                                                                 .OrderByDescending(x => x.Score)
                                                                                                 .ThenBy(x => x.LastUpdateTime)
                                                                                                 .ToArrayAsync();
            List<CompetitionRewardModel> rewards = await context.CompetitionReward.Where(x => x.CompetitionId == competitionID)
                                                                                   .ToListAsync();
            await GrantRewards(context, participations, rewards);

            await context.SaveChangesAsync();
        }

        private async Task GrantRewards(BaseDbContext context, PlayerParticipationModel[] participations, IEnumerable<CompetitionRewardModel> rewards)
        {
            foreach (CompetitionRewardModel reward in rewards)
            {
                GrantCondition cond = reward.GetCondition();
                if (cond is RankGrantCondition rankCondition)
                {
                    await GrantByCondition(context, reward, participations, rankCondition);
                }
                else if (cond is PlaceGrantCondition placeCondition)
                {
                    await GrantByCondition(context, reward, participations, placeCondition);
                }
                else
                {
                    logger.LogError($"Unspecified rank condition \"{cond.Type}\", skipping");
                }
            }
        }

        private static async Task GrantByCondition(BaseDbContext context, CompetitionRewardModel model, PlayerParticipationModel[] participations, PlaceGrantCondition placeCondition)
        {
            if (participations.Length == 0)
            {
                return;
            }

            int competitionID = participations[0].CompetitionID;
            int startIdx = placeCondition.MinPlace <= 0 ? 0 : placeCondition.MinPlace - 1;
            for (int i = startIdx; i < placeCondition.MaxPlace && i < participations.Length; i++)
            {
                await context.PlayerReward.AddAsync(new PlayerRewardModel(participations[i].AccountID, model.RewardDescriptionId, competitionID));
            }
        }

        private static async Task GrantByCondition(BaseDbContext context, CompetitionRewardModel model, PlayerParticipationModel[] participations, RankGrantCondition rankCondition)
        {
            if (participations.Length == 0)
            {
                return;
            }

            int minPlace = (int)Math.Floor(participations.Length * (1.0 - rankCondition.MaxRank)) + 1;
            int maxPlace = (int)Math.Ceiling(participations.Length * (1.0 - rankCondition.MinRank)) + 1;
            await GrantByCondition(context, model, participations, new PlaceGrantCondition(minPlace, maxPlace));
        }
    }
}
