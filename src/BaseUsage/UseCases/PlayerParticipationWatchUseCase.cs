using System.Text;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class PlayerParticipationWatchUseCase : IPlayerParticipationWatchUseCase
    {
        private readonly IPlayerParticipationService service;
        public PlayerParticipationWatchUseCase(IPlayerParticipationService service)
        {
            this.service = service;
        }

        public async Task<IEnumerable<PlayerParticipationDTO>> GetLeaderboard(int competition, DataLimiterDTO limiter)
        {
            return from n in await service.GetLeaderboard(competition, limiter.Convert()) select n.Convert();
        }

        public async Task<PlayerParticipationDTO> GetParticipation(int competition, int accountID)
        {
            return (await service.GetParticipation(accountID, competition)).Convert();
        }
    }

    public class PlayerParticipationUseCase : BaseAuthableUseCase<PlayerParticipationUseCase>, IPlayerParticipationUseCase
    {
        private readonly IPlayerParticipationService service;
        public PlayerParticipationUseCase(IAuthService authService, IPlayerParticipationService service)
            : base(authService)
        {
            this.service = service;
        }

        public async Task DeleteParticipation(int competition, int accountID)
        {
            AdminAuthCheck(out _);
            await service.DeleteParticipation(competition, accountID);
        }

        public async Task<IEnumerable<PlayerParticipationDTO>> GetMyParticipations(DataLimiterDTO limiter)
        {
            PlayerAuthCheck(out int id);
            return from n in await service.GetPlayerParticipations(id, limiter.Convert()) select n.Convert();
        }

        [Obsolete("Defunct")]
        public async Task SubmitScoreTo(int competition, int score)
        {
            PlayerAuthCheck(out int id);
            await service.SubmitParticipation(id, competition, score);
        }

        async Task<IPlayerParticipationUseCase> IAuthableUseCase<IPlayerParticipationUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
