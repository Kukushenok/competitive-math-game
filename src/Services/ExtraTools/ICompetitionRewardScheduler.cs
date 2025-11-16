using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services.ExtraTools
{
    public interface ICompetitionRewardScheduler
    {
        public Task OnCompetitionCreated(Competition c);
        public Task OnCompetitionUpdated(Competition c);
    }
}
