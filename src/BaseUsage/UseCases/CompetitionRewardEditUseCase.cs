using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class CompetitionRewardEditUseCase : BaseAuthableUseCase<CompetitionRewardEditUseCase>, ICompetitionRewardEditUseCase
    {
        private ICompetitionRewardService _service;
        public CompetitionRewardEditUseCase(IAuthService authService, ICompetitionRewardService service) : base(authService)
        {
            _service = service;
        }

        public async Task CreateCompetitionReward(CreateCompetitionRewardDTO reward)
        {
            AdminAuthCheck(out _);
            await _service.CreateCompetitionReward(reward.Extend().Convert());
        }

        public async Task RemoveCompetitionReward(int compRewardID)
        {
            AdminAuthCheck(out _);
            await _service.RemoveCompetitionReward(compRewardID);
        }

        public async Task UpdateCompetitionReward(UpdateCompetitionRewardDTO reward)
        {
            AdminAuthCheck(out _);
            if (reward.ID == null) throw new InvalidInputDataException("The ID for patched reward is null");
            await _service.UpdateCompetitionReward(reward.ID!.Value, reward.RewardDescriptionID, reward.GetCondition());
        }

        async Task<ICompetitionRewardEditUseCase> IAuthableUseCase<ICompetitionRewardEditUseCase>.Auth(string token) => await Auth(token);
    }
}
