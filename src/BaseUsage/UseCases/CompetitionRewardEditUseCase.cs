using System.Text;
using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class CompetitionRewardEditUseCase : BaseAuthableUseCase<CompetitionRewardEditUseCase>, ICompetitionRewardEditUseCase
    {
        private readonly ICompetitionRewardService service;
        public CompetitionRewardEditUseCase(IAuthService authService, ICompetitionRewardService service)
            : base(authService)
        {
            this.service = service;
        }

        public async Task CreateCompetitionReward(CreateCompetitionRewardDTO reward)
        {
            AdminAuthCheck(out _);
            await service.CreateCompetitionReward(reward.Extend().Convert());
        }

        public async Task RemoveCompetitionReward(int compRewardID)
        {
            AdminAuthCheck(out _);
            await service.RemoveCompetitionReward(compRewardID);
        }

        public async Task UpdateCompetitionReward(UpdateCompetitionRewardDTO reward)
        {
            AdminAuthCheck(out _);
            if (reward.ID == null)
            {
                throw new InvalidInputDataException("The ID for patched reward is null");
            }

            await service.UpdateCompetitionReward(reward.ID!.Value, reward.RewardDescriptionID, reward.GetCondition());
        }

        async Task<ICompetitionRewardEditUseCase> IAuthableUseCase<ICompetitionRewardEditUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
