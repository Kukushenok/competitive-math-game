using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerParticipationUseCase: IAuthableUseCase<IPlayerParticipationUseCase>
    {
        public Task SubmitScoreTo(int competition, int score);
        public Task DeleteParticipation(int competition, int accountID);
        public Task<IEnumerable<PlayerParticipationDTO>> GetMyParticipations(DataLimiterDTO limiter);
    }
}
