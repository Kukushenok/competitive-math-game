using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Objects;
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
        private const string SCHED_CATEGORY = "Competition";
        private IServiceProvider provider;
        protected ITimeScheduler _timeScheduler;
        public CompetitionRewardScheduler(IServiceProvider provider, ITimeScheduler scheduler)
        {
            this.provider = provider;
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
            if (data.Category == SCHED_CATEGORY)
            {
                using (var scope = provider.CreateScope())
                {
                    var _rewardRepository = scope.ServiceProvider.GetRequiredService<IPlayerRewardRepository>();
                    var _repositoryPrivilegySetting = scope.ServiceProvider.GetRequiredService<IRepositoryPrivilegySetting>();
                    _repositoryPrivilegySetting.SetPrivilegies("RewardScheduler");
                    await _rewardRepository.GrantRewardsFor(data.Identifier);
                }
            }
        }
        private TimeScheduledTaskData GetSchedTask(Competition c)
        {
            return new TimeScheduledTaskData(c.Id!.Value, SCHED_CATEGORY, new DateTimeOffset(c.EndDate), c.Name);
        }
    }
}
