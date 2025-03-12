using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.CompetitionService
{
    public interface ICompetitionRewardScheduler
    {
        public void OnCompetitionCreated(Competition c);
        public void OnCompetitionUpdated(Competition c);
    }
    public class CompetitionRewardScheduler : ICompetitionRewardScheduler
    {
        protected IPlayerRewardRepository _rewardRepository;
        public CompetitionRewardScheduler(IPlayerRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public void OnCompetitionCreated(Competition c)
        {
            // cron to  c.EndDate;
        }

        public void OnCompetitionUpdated(Competition c)
        {
            // remove some cron, add cron to  c.EndDate;
        }
    }
}
