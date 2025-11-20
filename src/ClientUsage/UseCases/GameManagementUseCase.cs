using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class GameManagementUseCase : AuthableUseCaseBase<IGameManagementUseCase>, IGameManagementUseCase
    {
        public GameManagementUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<IGameManagementUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            IGameManagementUseCase impl = new GameManagementUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task<IEnumerable<RiddleInfoDTO>> GetRiddles(int competitionID, DataLimiterDTO limiter)
        {
            return client.Get<IEnumerable<RiddleInfoDTO>>($"/api/v1/competitions/{competitionID}/riddles?page={limiter.Page}&count={limiter.Count}");
        }

        public Task CreateRiddle(RiddleInfoDTO riddle)
        {
            return client.PostNoContent($"/api/v1/competitions/{riddle.CompetitionID}/riddles", riddle);
        }

        public Task UpdateRiddle(RiddleInfoDTO riddle)
        {
            return client.PutNoContent($"/api/v1/competitions/{riddle.CompetitionID}/riddles/{riddle.ID}", riddle);
        }

        public Task DeleteRiddle(int riddleID)
        {
            return client.DeleteNoContent($"/api/v1/competitions/0/riddles/{riddleID}"); // Needs competition id; API requires compID
        }

        public Task<RiddleGameSettingsDTO> GetSettings(int competitionID)
        {
            return client.Get<RiddleGameSettingsDTO>($"/api/v1/competitions/{competitionID}/game_settings");
        }

        public Task UpdateSettings(int competitionID, RiddleGameSettingsDTO settings)
        {
            return client.PutNoContent($"/api/v1/competitions/{competitionID}/game_settings", settings);
        }
    }
}
