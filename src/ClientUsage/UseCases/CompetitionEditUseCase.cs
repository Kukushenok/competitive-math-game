// File: IHttpClient.cs
using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs
namespace ClientUsage.UseCases
{
    internal sealed class CompetitionEditUseCase : AuthableUseCaseBase<ICompetitionEditUseCase>, ICompetitionEditUseCase
    {
        public CompetitionEditUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<ICompetitionEditUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            ICompetitionEditUseCase impl = new CompetitionEditUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task CreateCompetition(CompetitionDTO competition)
        {
            return competition == null
                ? throw new ArgumentNullException(nameof(competition))
                : client.PostNoContent("/api/v1/competitions", competition);
        }

        public Task UpdateCompetition(CompetitionPatchRequestDTO competition)
        {
            ArgumentNullException.ThrowIfNull(competition);

            int? id = competition.ID;
            return client.PatchNoContent($"/api/v1/competitions/{id}", competition);
        }
    }
}
