using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class PlayerParticipationWatchUseCase : IPlayerParticipationWatchUseCase
    {
        private IPlayerParticipationService _service;
        public PlayerParticipationWatchUseCase(IPlayerParticipationService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<PlayerParticipationDTO>> GetLeaderboard(int competition, DataLimiterDTO limiter)
        {
            return from n in await _service.GetLeaderboard(competition, limiter.Convert()) select n.Convert();
        }

        public async Task<PlayerParticipationDTO> GetParticipation(int competition, int accountID)
        {
            return (await _service.GetParticipation(accountID, competition)).Convert();
        }
    }
    public class PlayerParticipationUseCase : BaseAuthableUseCase<PlayerParticipationUseCase>, IPlayerParticipationUseCase
    {
        private IPlayerParticipationService _service;
        public PlayerParticipationUseCase(IAuthService authService, IPlayerParticipationService service) : base(authService)
        {
            _service = service;
        }

        public async Task DeleteParticipation(int competition, int accountID)
        {
            AdminAuthCheck(out _);
            await _service.DeleteParticipation(competition, accountID);   
        }

        public async Task<IEnumerable<PlayerParticipationDTO>> GetMyParticipations(DataLimiterDTO limiter)
        {
            PlayerAuthCheck(out int id);
            return from n in await _service.GetPlayerParticipations(id, limiter.Convert()) select n.Convert();
        }

        public async Task SubmitScoreTo(int competition, int score)
        {
            PlayerAuthCheck(out int id);
            await _service.SubmitParticipation(id, competition, score);
        }

        async Task<IPlayerParticipationUseCase> IAuthableUseCase<IPlayerParticipationUseCase>.Auth(string token) => await Auth(token);
    }
}
