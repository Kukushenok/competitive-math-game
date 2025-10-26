using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.PlayerRewardService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    // TODO
    public class PlayerRewardServiceTests
    {
        private readonly PlayerRewardService service;
        private readonly Mock<IPlayerRewardRepository> rewardRepo;
        private readonly Mock<IRewardDescriptionRepository> descriptionRepo;
        public PlayerRewardServiceTests()
        {
            rewardRepo = new Mock<IPlayerRewardRepository>();
            descriptionRepo = new Mock<IRewardDescriptionRepository>();
            service = new PlayerRewardService(rewardRepo.Object, descriptionRepo.Object);
        }

        [Fact]
        public async Task PlayerRewardServiceTestGetAllRewardsOf()
        {
            // Arrange
            var etalon = new DataLimiter(10, 10);
            List<PlayerReward> data = [new PlayerReward(0, 11, "a", "b", 0, null, 0)];
            rewardRepo.Setup(x => x.GetAllRewardsOf(0, It.Is<DataLimiter>(p => etalon.Equals(p))))
                .Returns(() => Task.FromResult<IEnumerable<PlayerReward>>(data));

            // Act
            IEnumerable<PlayerReward> p = await service.GetAllRewardsOf(0, etalon);

            // Assert
            Assert.Equal(data, p);
        }

        [Fact]
        public async Task PlayerRewardServiceTestDeleteReward()
        {
            // Act
            await service.DeleteReward(0);

            // Assert
            rewardRepo.Verify(x => x.DeleteReward(0), Times.Once);
        }

        [Fact]
        public async Task PlayerRewardServiceTestGrantRewardToPlayer()
        {
            // Arrange
            descriptionRepo.Setup(x => x.GetRewardDescription(0)).ReturnsAsync(new RewardDescription("AAA", "BBB", 0));

            // Act
            await service.GrantRewardToPlayer(0, 0);

            // Assert
            rewardRepo.Verify(
                x => x.CreateReward(It.Is<PlayerReward>(
                a => a.Name == "AAA" && a.Description == "BBB" && a.RewardDescriptionID == 0)),
                Times.Once);
        }

        [Fact]
        public async Task PlayerRewardServiceTestGrantRewardToPlayerFailure()
        {
            // Arrange
            descriptionRepo.Setup(x => x.GetRewardDescription(0)).Throws(new MissingDataException());

            // Act Assert
            await Assert.ThrowsAsync<MissingDataException>(async () => await service.GrantRewardToPlayer(0, 0));
        }
    }
}
