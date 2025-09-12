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
            // Arrange
            DateTime dt = DateTime.Now;
            Competition c = new Competition("Sample", "descr", dt, dt + TimeSpan.FromSeconds(10), 5);

            // Act
            await scheduler.OnCompetitionUpdated(c);
            
            // Assert
            _timeScheduler.Verify(x => x.AddOrUpdateScheduledTask(It.Is<TimeScheduledTaskData>(
                 p => p.Identifier == 5 && p.FireTime.Equals(new DateTimeOffset(c.EndDate)))), Times.Once);
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnCompetitionCreated()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition c = new Competition("Sample", "descr", dt, dt + TimeSpan.FromSeconds(10), 5);

            // Act
            await scheduler.OnCompetitionCreated(c);

            // Assert
            _timeScheduler.Verify(x => x.AddOrUpdateScheduledTask(It.Is<TimeScheduledTaskData>(
               p => p.Identifier == 5 && p.FireTime.Equals(new DateTimeOffset(c.EndDate)))), Times.Once);
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnRecievedMessage_Ok()
        {
            // Arrange
            TimeScheduledTaskData data = new TimeScheduledTaskData(0, "Competition", DateTimeOffset.Now, "Hello");

            // Act
            await scheduler.OnRecievedMessage(data);

            // Assert
            _rewardRepo.Verify(x => x.GrantRewardsFor(0), Times.Once);
        }
        [Fact]
        public async Task CompetitionRewardSchedulerTests_OnRecievedMessage_Skip()
        {
            // Arrange
            TimeScheduledTaskData data = new TimeScheduledTaskData(0, "Flexio", DateTimeOffset.Now, "Hello");

            // Act
            await scheduler.OnRecievedMessage(data);

            // Assert
            _rewardRepo.Verify(x => x.GrantRewardsFor(0), Times.Never);
        }
    }
}
