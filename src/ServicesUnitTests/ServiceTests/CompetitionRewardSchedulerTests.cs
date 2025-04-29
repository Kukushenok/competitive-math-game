using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Repositories.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionRewardSchedulerTests
    {
        private Mock<ITimeScheduler> _timeScheduler;
        private Mock<IPlayerRewardRepository> _rewardRepo;
        private Mock<IRepositoryPrivilegySetting> _setting;
        private CompetitionRewardScheduler scheduler;
        public CompetitionRewardSchedulerTests()
        {
            _rewardRepo = new Mock<IPlayerRewardRepository>();
            _timeScheduler = new Mock<ITimeScheduler>();
            _setting = new Mock<IRepositoryPrivilegySetting>();
            ServiceCollection p = new ServiceCollection();
            p.AddScoped((p) => _rewardRepo.Object);
            p.AddScoped((p) => _setting.Object);
            scheduler = new CompetitionRewardScheduler(p.BuildServiceProvider(), _timeScheduler.Object);
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnCompetitionUpdated()
        {
            DateTime dt = DateTime.Now;
            Competition c = new Competition("Sample", "descr", dt, dt + TimeSpan.FromSeconds(10), 5);
            bool called = false;
            _timeScheduler.Setup(x => x.AddOrUpdateScheduledTask(It.IsAny<TimeScheduledTaskData>()))
                .Callback<TimeScheduledTaskData>((x) =>
                {
                    Assert.Equal(5, x.Identifier);
                    Assert.Equal(new DateTimeOffset(c.EndDate), x.FireTime);
                    called = true;
                });
            await scheduler.OnCompetitionUpdated(c);
            Assert.True(called, "Why it is not adding things?");
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnCompetitionCreated()
        {
            DateTime dt = DateTime.Now;
            Competition c = new Competition("Sample", "descr", dt, dt + TimeSpan.FromSeconds(10), 5);
            bool called = false;
            _timeScheduler.Setup(x => x.AddOrUpdateScheduledTask(It.IsAny<TimeScheduledTaskData>()))
                .Callback<TimeScheduledTaskData>((x) =>
                {
                    Assert.Equal(5, x.Identifier);
                    Assert.Equal(new DateTimeOffset(c.EndDate), x.FireTime);
                    called = true;
                });
            await scheduler.OnCompetitionCreated(c);
            Assert.True(called, "Why it is not adding things?");
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnRecievedMessage_Ok()
        {
            TimeScheduledTaskData data = new TimeScheduledTaskData(0, "Competition", DateTimeOffset.Now, "Hello");
            bool called = false;
            _rewardRepo.Setup(x => x.GrantRewardsFor(0)).Callback(() => called = true);
            await scheduler.OnRecievedMessage(data);
            Assert.True(called, "Should recieve messages within Competition category");
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnRecievedMessage_Skip()
        {
            TimeScheduledTaskData data = new TimeScheduledTaskData(0, "Flexio", DateTimeOffset.Now, "Hello");
            bool called = false;
            _rewardRepo.Setup(x => x.GrantRewardsFor(0)).Callback(() => called = true);
            await scheduler.OnRecievedMessage(data);
            Assert.False(called, "Should recieve messages only within Competition category (the category was Flexio)");
        }
    }
}
