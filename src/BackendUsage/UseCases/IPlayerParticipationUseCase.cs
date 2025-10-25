using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerParticipationUseCase : IAuthableUseCase<IPlayerParticipationUseCase>
    {
        [System.Obsolete("Not the API I Know.")]
        Task SubmitScoreTo(int competition, int score);
        Task DeleteParticipation(int competition, int accountID);
        Task<IEnumerable<PlayerParticipationDTO>> GetMyParticipations(DataLimiterDTO limiter);
    }
}
