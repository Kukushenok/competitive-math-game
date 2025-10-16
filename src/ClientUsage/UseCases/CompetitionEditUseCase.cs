// File: IHttpClient.cs
using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs

namespace ClientUsage.UseCases
{
    internal class CompetitionEditUseCase : AuthableUseCaseBase<ICompetitionEditUseCase>, ICompetitionEditUseCase
    {
        public CompetitionEditUseCase(IHttpClient client) : base(client) { }

        public override Task<ICompetitionEditUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            ICompetitionEditUseCase impl = new CompetitionEditUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task CreateCompetition(CompetitionDTO competition)
        {
            if (competition == null) throw new ArgumentNullException(nameof(competition));
            return _client.PostNoContent("/api/v1/competitions", competition);
        }

        public Task UpdateCompetition(CompetitionPatchRequestDTO competition)
        {
            if (competition == null) throw new ArgumentNullException(nameof(competition));
            var id = competition.ID;
            return _client.PatchNoContent($"/api/v1/competitions/{id}", competition);
        }
    }
}
