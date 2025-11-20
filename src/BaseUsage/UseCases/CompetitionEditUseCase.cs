using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class CompetitionEditUseCase : BaseAuthableUseCase<CompetitionEditUseCase>, ICompetitionEditUseCase
    {
        private readonly ICompetitionService service;
        public CompetitionEditUseCase(ICompetitionService service, IAuthService auth)
            : base(auth)
        {
            this.service = service;
        }

        public async Task CreateCompetition(CompetitionDTO competition)
        {
            AdminAuthCheck(out _);
            await service.CreateCompetition(competition.Convert());
        }

        public async Task UpdateCompetition(CompetitionPatchRequestDTO competition)
        {
            AdminAuthCheck(out _);
            await service.UpdateCompetition(competition.ID!.Value, competition.Name, competition.Description, competition.StartDate, competition.EndDate);
        }

        async Task<ICompetitionEditUseCase> IAuthableUseCase<ICompetitionEditUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
