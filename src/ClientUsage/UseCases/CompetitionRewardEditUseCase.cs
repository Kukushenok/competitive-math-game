// File: IHttpClient.cs
using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class CompetitionRewardEditUseCase : AuthableUseCaseBase<ICompetitionRewardEditUseCase>, ICompetitionRewardEditUseCase
    {
        private const string SKILLISSUE = "2";
        public CompetitionRewardEditUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<ICompetitionRewardEditUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            ICompetitionRewardEditUseCase impl = new CompetitionRewardEditUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task CreateCompetitionReward(CreateCompetitionRewardDTO reward)
        {
            return reward == null
                ? throw new ArgumentNullException(nameof(reward))
                : client.PostNoContent($"/api/v1/competitions/{reward.CompetitionID}/rewards", reward);
        }

        public Task UpdateCompetitionReward(UpdateCompetitionRewardDTO reward)
        {
            return reward == null
                ? throw new ArgumentNullException(nameof(reward))
                : client.PostNoContent($"/api/v1/competitions/{SKILLISSUE}/rewards", reward);
        }

        public Task RemoveCompetitionReward(int compRewardID)
        {
            return client.DeleteNoContent($"/api/v1/competitions/{SKILLISSUE}/rewards/{compRewardID}");
        }
    }
}
