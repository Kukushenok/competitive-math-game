using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.PlayerRewardService;
using Moq;

namespace ServiceUnitTests
{
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
            DataLimiter etalon = new DataLimiter(10, 10);
            List<PlayerReward> data = new List<PlayerReward>() { new PlayerReward(0, 11, "a", "b", 0, null, 0) };
            _rewardRepo.Setup(x => x.GetAllRewardsOf(0, It.IsAny<DataLimiter>())).Callback<int, DataLimiter>
                ((idx, limiter) =>
                {
                    Assert.Equal(0, idx);
                    Assert.Equal(etalon, limiter);
                }).Returns(() => Task.FromResult<IEnumerable<PlayerReward>>(data));
            IEnumerable<PlayerReward> p = await _service.GetAllRewardsOf(0, etalon);
            Assert.Equal(data, p);
        }
        [Fact]
        public async Task PlayerRewardServiceTest_DeleteReward()
        {
            _rewardRepo.Setup(x => x.DeleteReward(It.IsAny<int>())).Callback<int>(
                (idx) =>
                {
                    Assert.Equal(0, idx);
                });
            await _service.DeleteReward(0);
        }
        [Fact]
        public async Task PlayerRewardServiceTest_GrantRewardToPlayer()
        {
            _descriptionRepo.Setup(x => x.GetRewardDescription(0)).ReturnsAsync(new RewardDescription("AAA", "BBB", 0));
            _rewardRepo.Setup(x => x.CreateReward(It.IsAny<PlayerReward>())).Callback<PlayerReward>(
                (a) =>
                {
                    Assert.Equal("AAA", a.Name);
                    Assert.Equal("BBB", a.Description);
                    Assert.Equal(0, a.RewardDescriptionID);
                });
            await _service.GrantRewardToPlayer(0, 0);
        }
        [Fact]
        public async Task PlayerRewardServiceTEst_GrantRewardToPlayer_Failure()
        {
            _descriptionRepo.Setup(x => x.GetRewardDescription(0)).Throws(new MissingDataException());
            await Assert.ThrowsAsync<MissingDataException>(async () => await _service.GrantRewardToPlayer(0, 0));
        }
    }
}
