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
        private PlayerRewardService _service;
        private Mock<IPlayerRewardRepository> _rewardRepo;
        private Mock<IRewardDescriptionRepository> _descriptionRepo;
        public PlayerRewardServiceTests()
        {
            _rewardRepo = new Mock<IPlayerRewardRepository>();
            _descriptionRepo = new Mock<IRewardDescriptionRepository>();
            _service = new PlayerRewardService(_rewardRepo.Object, _descriptionRepo.Object);
        }
        [Fact]
        public async Task PlayerRewardServiceTest_GetAllRewardsOf()
        {
            // Arrange
            DataLimiter etalon = new DataLimiter(10, 10);
            List<PlayerReward> data = new List<PlayerReward>() { new PlayerReward(0, 11, "a", "b", 0, null, 0) };
            _rewardRepo.Setup(x => x.GetAllRewardsOf(0, It.Is<DataLimiter>(p => etalon.Equals(p))))
                .Returns(() => Task.FromResult<IEnumerable<PlayerReward>>(data));
            // Act
            IEnumerable<PlayerReward> p = await _service.GetAllRewardsOf(0, etalon);
            // Assert
            Assert.Equal(data, p);
        }
        [Fact]
        public async Task PlayerRewardServiceTest_DeleteReward()
        {
            // Act
            await _service.DeleteReward(0);
            // Assert
            _rewardRepo.Verify(x => x.DeleteReward(0), Times.Once);
        }
        [Fact]
        public async Task PlayerRewardServiceTest_GrantRewardToPlayer()
        {
            // Arrange
            _descriptionRepo.Setup(x => x.GetRewardDescription(0)).ReturnsAsync(new RewardDescription("AAA", "BBB", 0));
            // Act
            await _service.GrantRewardToPlayer(0, 0);
            // Assert
            _rewardRepo.Verify(x => x.CreateReward(It.Is<PlayerReward>(
                a => "AAA" == a.Name && "BBB" == a.Description && 0 == a.RewardDescriptionID)
            ), Times.Once);
        }
        [Fact]
        public async Task PlayerRewardServiceTest_GrantRewardToPlayer_Failure()
        {
            // Arrange
            _descriptionRepo.Setup(x => x.GetRewardDescription(0)).Throws(new MissingDataException());
            // Act Assert
            await Assert.ThrowsAsync<MissingDataException>(async () => await _service.GrantRewardToPlayer(0, 0));
        }
    }
}
