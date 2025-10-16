using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class GameManagementUseCase : AuthableUseCaseBase<IGameManagementUseCase>, IGameManagementUseCase
    {
        public GameManagementUseCase(IHttpClient client) : base(client) { }

        public override Task<IGameManagementUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            IGameManagementUseCase impl = new GameManagementUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task<IEnumerable<RiddleInfoDTO>> GetRiddles(int competitionID, DataLimiterDTO limiter)
            => _client.Get<IEnumerable<RiddleInfoDTO>>($"/api/v1/competitions/{competitionID}/riddles?page={limiter.Page}&count={limiter.Count}");

        public Task CreateRiddle(RiddleInfoDTO riddle)
            => _client.PostNoContent($"/api/v1/competitions/{riddle.CompetitionID}/riddles", riddle);

        public Task UpdateRiddle(RiddleInfoDTO riddle)
            => _client.PutNoContent($"/api/v1/competitions/{riddle.CompetitionID}/riddles/{riddle.ID}", riddle);

        public Task DeleteRiddle(int riddleID)
            => _client.DeleteNoContent($"/api/v1/competitions/0/riddles/{riddleID}"); // Needs competition id; API requires compID

        public Task<RiddleGameSettingsDTO> GetSettings(int competitionID)
            => _client.Get<RiddleGameSettingsDTO>($"/api/v1/competitions/{competitionID}/game_settings");

        public Task UpdateSettings(int competitionID, RiddleGameSettingsDTO settings)
            => _client.PutNoContent($"/api/v1/competitions/{competitionID}/game_settings", settings);
    }
}
