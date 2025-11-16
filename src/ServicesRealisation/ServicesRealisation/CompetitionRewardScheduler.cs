using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Objects;

namespace CompetitiveBackend.Services.CompetitionService
{
    public class CompetitionRewardScheduler : ICompetitionRewardScheduler, ITimeScheduledTaskSubscriber
    {
        private const string SCHEDCATEGORY = "Competition";
        private const string SCHEDPRIVILEGIES = "RewardScheduler";
        private readonly IServiceProvider provider;
        protected ITimeScheduler timeScheduler;
        public CompetitionRewardScheduler(IServiceProvider provider, ITimeScheduler scheduler)
        {
            this.provider = provider;
            timeScheduler = scheduler;
            timeScheduler.AddSubscriber(this);
        }

        public void Dispose()
        {
            timeScheduler.RemoveSubscriber(this);
        }

        public async Task OnCompetitionCreated(Competition c)
        {
            await timeScheduler.AddOrUpdateScheduledTask(GetSchedTask(c));
        }

        public async Task OnCompetitionUpdated(Competition c)
        {
            await timeScheduler.AddOrUpdateScheduledTask(GetSchedTask(c));
        }

        public async Task OnRecievedMessage(TimeScheduledTaskData data)
        {
            if (data.Category == SCHEDCATEGORY)
            {
                using IServiceScope scope = provider.CreateScope();
                IPlayerRewardRepository rewardRepository = scope.ServiceProvider.GetRequiredService<IPlayerRewardRepository>();
                IRepositoryPrivilegySetting repositoryPrivilegySetting = scope.ServiceProvider.GetRequiredService<IRepositoryPrivilegySetting>();
                repositoryPrivilegySetting.SetPrivilegies(SCHEDPRIVILEGIES);
                await rewardRepository.GrantRewardsFor(data.Identifier);
            }
        }

        private static TimeScheduledTaskData GetSchedTask(Competition c)
        {
            return new TimeScheduledTaskData(c.Id!.Value, SCHEDCATEGORY, new DateTimeOffset(c.EndDate), c.Name);
        }
    }
}
