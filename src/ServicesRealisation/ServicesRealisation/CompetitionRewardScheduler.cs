using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using System.Threading.Tasks;

namespace CompetitiveBackend.Services.CompetitionService
{
    public interface ICompetitionRewardScheduler
    {
        public Task OnCompetitionCreated(Competition c);
        public Task OnCompetitionUpdated(Competition c);
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
        
        public async Task OnCompetitionCreated(Competition c)
        {
            await _timeScheduler.AddOrUpdateScheduledTask(GetSchedTask(c));
        }

        public async Task OnCompetitionUpdated(Competition c)
        {
            await _timeScheduler.AddOrUpdateScheduledTask(GetSchedTask(c));
        }

        public async Task OnRecievedMessage(TimeScheduledTaskData data)
        {
            if(data.Category == "Competition")
                await _rewardRepository.GrantRewardsFor(data.Identifier);
        }
        private TimeScheduledTaskData GetSchedTask(Competition c)
        {
            return new TimeScheduledTaskData(c.Id!.Value, "Competition", new DateTimeOffset(c.EndDate), c.Name);
        }
    }
}
