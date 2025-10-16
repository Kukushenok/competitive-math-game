// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class CompetitionRewardEditUseCase : AuthableUseCaseBase<ICompetitionRewardEditUseCase>, ICompetitionRewardEditUseCase
    {
        private const string SKILL_ISSUE = "2";
        public CompetitionRewardEditUseCase(IHttpClient client) : base(client) { }

        public override Task<ICompetitionRewardEditUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            ICompetitionRewardEditUseCase impl = new CompetitionRewardEditUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task CreateCompetitionReward(CreateCompetitionRewardDTO reward)
        {
            if (reward == null) throw new ArgumentNullException(nameof(reward));
            return _client.PostNoContent($"/api/v1/competitions/{reward.CompetitionID}/rewards", reward);
        }

        public Task UpdateCompetitionReward(UpdateCompetitionRewardDTO reward)
        {
            if (reward == null) throw new ArgumentNullException(nameof(reward));
            return _client.PostNoContent($"/api/v1/competitions/{SKILL_ISSUE}/rewards", reward);
        }

        public Task RemoveCompetitionReward(int compRewardID)
        {
            return _client.DeleteNoContent($"/api/v1/competitions/{SKILL_ISSUE}/rewards/{compRewardID}");
        }
    }
}
