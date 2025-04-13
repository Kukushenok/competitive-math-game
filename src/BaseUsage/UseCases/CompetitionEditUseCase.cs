using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class CompetitionEditUseCase : BaseAuthableUseCase<CompetitionEditUseCase>, ICompetitionEditUseCase
    {
        private ICompetitionService _service;
        public CompetitionEditUseCase(ICompetitionService service, IAuthService auth): base(auth)
        {
            _service = service;
        }

        public async Task CreateCompetition(CompetitionDTO competition)
        {
            AdminAuthCheck(out _);
            await _service.CreateCompetition(competition.Convert());
        }

        public async Task SetCompetitionLevel(int competitionID, LargeDataDTO levelData)
        {
            AdminAuthCheck(out _);
            await _service.SetCompetitionLevel(competitionID, levelData.Convert());
        }

        public async Task UpdateCompetition(CompetitionUpdateRequestDTO competition)
        {
            AdminAuthCheck(out _);
            await _service.UpdateCompetition(competition.ID!.Value, competition.Name, competition.Description, competition.StartDate, competition.EndDate);
        }

        async Task<ICompetitionEditUseCase> IAuthableUseCase<ICompetitionEditUseCase>.Auth(string token) => await Auth(token);
    }
}
