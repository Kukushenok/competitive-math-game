#if DISABLED
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Repositories.Objects;

namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionRewardSchedulerTests : IDisposable
    {
        private readonly Mock<ITimeScheduler> timeScheduler;
        private readonly Mock<IPlayerRewardRepository> rewardRepo;
        private readonly Mock<IRepositoryPrivilegySetting> setting;
        private readonly CompetitionRewardScheduler scheduler;
        public CompetitionRewardSchedulerTests()
        {
            rewardRepo = new Mock<IPlayerRewardRepository>();
            timeScheduler = new Mock<ITimeScheduler>();
            setting = new Mock<IRepositoryPrivilegySetting>();
            var p = new ServiceCollection();
            p.AddScoped((p) => rewardRepo.Object);
            p.AddScoped((p) => setting.Object);
            scheduler = new CompetitionRewardScheduler(p.BuildServiceProvider(), timeScheduler.Object);
        }

        [Fact]
        public async Task CompetitionRewardSchedulerTestsOnCompetitionUpdated()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var c = new Competition("Sample", "descr", dt, dt + TimeSpan.FromSeconds(10), 5);

            // Act
            await scheduler.OnCompetitionUpdated(c);

            // Assert
            timeScheduler.Verify(
                x => x.AddOrUpdateScheduledTask(It.Is<TimeScheduledTaskData>(
                 p => p.Identifier == 5 && p.FireTime.Equals(new DateTimeOffset(c.EndDate)))),
                Times.Once);
        }

        [Fact]
        public async Task CompetitionRewardSchedulerTestsOnCompetitionCreated()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var c = new Competition("Sample", "descr", dt, dt + TimeSpan.FromSeconds(10), 5);

            // Act
            await scheduler.OnCompetitionCreated(c);

            // Assert
            timeScheduler.Verify(
                x => x.AddOrUpdateScheduledTask(It.Is<TimeScheduledTaskData>(
               p => p.Identifier == 5 && p.FireTime.Equals(new DateTimeOffset(c.EndDate)))),
                Times.Once);
        }

        [Fact]
        public async Task CompetitionRewardSchedulerTestsOnRecievedMessageOk()
        {
            // Arrange
            var data = new TimeScheduledTaskData(0, "Competition", DateTimeOffset.Now, "Hello");

            // Act
            await scheduler.OnRecievedMessage(data);

            // Assert
            rewardRepo.Verify(x => x.GrantRewardsFor(0), Times.Once);
        }

        [Fact]
        public async Task CompetitionRewardSchedulerTestsOnRecievedMessageSkip()
        {
            // Arrange
            var data = new TimeScheduledTaskData(0, "Flexio", DateTimeOffset.Now, "Hello");

            // Act
            await scheduler.OnRecievedMessage(data);

            // Assert
            rewardRepo.Verify(x => x.GrantRewardsFor(0), Times.Never);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
#endif