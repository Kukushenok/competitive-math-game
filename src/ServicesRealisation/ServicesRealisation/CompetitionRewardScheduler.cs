using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using System.Threading.Tasks;

namespace CompetitiveBackend.Services.CompetitionService
{
    public interface ICompetitionRewardScheduler
    {
        public void OnCompetitionCreated(Competition c);
        public void OnCompetitionUpdated(Competition c);
    }
    public class CompetitionRewardScheduler : ICompetitionRewardScheduler, ITimeScheduledTaskSubscriber
    {
        protected IPlayerRewardRepository _rewardRepository;
        protected ITimeScheduler _timeScheduler;
        public CompetitionRewardScheduler(IPlayerRewardRepository rewardRepository, ITimeScheduler scheduler)
        {
            _rewardRepository = rewardRepository;
            _timeScheduler = scheduler;
            _timeScheduler.AddSubscriber(this);
        }

        public void Dispose()
        {
            _timeScheduler.RemoveSubscriber(this);
        }
        
        public void OnCompetitionCreated(Competition c)
        {
            _timeScheduler.AddOrUpdateScheduledTask(GetSchedTask(c));
        }

        public void OnCompetitionUpdated(Competition c)
        {
            _timeScheduler.AddOrUpdateScheduledTask(GetSchedTask(c));
        }

        public async Task OnRecievedMessage(TimeScheduledTaskData data)
        {
            if(data.Category == "Competition")
                await _rewardRepository.GrantRewardsFor(data.Identifier);
        }
        private TimeScheduledTaskData GetSchedTask(Competition c)
        {
            return new TimeScheduledTaskData(c.Id!.Value, "Competition", c.EndDate, c.Name);
        }
    }
}
