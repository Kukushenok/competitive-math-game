using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.CompetitionRewardService
{
    public class CompetitionRewardService : ICompetitionRewardService
    {
        private readonly ICompetitionRewardRepository repository;
        public CompetitionRewardService(ICompetitionRewardRepository rewardRepository)
        {
            repository = rewardRepository;
        }

        public async Task CreateCompetitionReward(CompetitionReward reward)
        {
            await repository.CreateCompetitionReward(reward);
        }

        public async Task<IEnumerable<CompetitionReward>> GetCompetitionRewards(int competitionID)
        {
            return await repository.GetCompetitionRewards(competitionID);
        }

        public async Task RemoveCompetitionReward(int rewardID)
        {
            await repository.RemoveCompetitionReward(rewardID);
        }
        public async Task UpdateCompetitionReward(int competitionRewardID, int? rewardDescriptionID, GrantCondition? condition)
        {
            CompetitionReward curr = await repository.GetCompetitionReward(competitionRewardID);

            CompetitionReward rwd = new CompetitionReward(
                rewardDescriptionID ?? curr.RewardDescriptionID,
                curr.CompetitionID,
                curr.Name,
                curr.Description,
                condition ?? curr.Condition,
                curr.Id
                );
            await repository.UpdateCompetitionReward(rwd);
        }
    }
}
