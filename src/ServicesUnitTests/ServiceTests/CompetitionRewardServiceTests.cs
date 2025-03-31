using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionRewardService;
using Core.RewardCondition;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionRewardServiceTests
    {
        private CompetitionRewardService _service;
        private Mock<ICompetitionRewardRepository> _rewardRepo;
        public CompetitionRewardServiceTests()
        {
            _rewardRepo = new Mock<ICompetitionRewardRepository>();
            _service = new CompetitionRewardService(_rewardRepo.Object);
        }
        [Fact]
        public async Task CompetitionRewardServiceTests_CreateCompetitionReward()
        {
            CompetitionReward etalon = new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1));
            _rewardRepo.Setup(x => x.CreateCompetitionReward(It.IsAny<CompetitionReward>())).Callback<CompetitionReward>(
                (reward) =>
                {
                    Assert.Equal(etalon.Name, reward.Name);
                    Assert.Equal(etalon.Condition, reward.Condition);
                    Assert.Equal(etalon.Description, reward.Description);
                    Assert.Equal(etalon.CompetitionID, reward.CompetitionID);
                    Assert.Equal(etalon.RewardDescriptionID, reward.RewardDescriptionID);
                });
            await _service.CreateCompetitionReward(etalon);
        }
        [Fact]
        public async Task CompetitionRewardServiceTests_UpdateCompetitionReward()
        {
            CompetitionReward etalon = new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1), 0);
            var placeGrant = new PlaceGrantCondition(1, 1);
            _rewardRepo.Setup(x => x.GetCompetitionReward(0)).ReturnsAsync(etalon);
            _rewardRepo.Setup(x => x.UpdateCompetitionReward(It.IsAny<CompetitionReward>())).Callback<CompetitionReward>(
                (reward) =>
                {
                    Assert.Equal(etalon.Name, reward.Name);
                    Assert.Equal(placeGrant, reward.Condition);
                    Assert.Equal(etalon.Description, reward.Description);
                    Assert.Equal(etalon.CompetitionID, reward.CompetitionID);
                    Assert.Equal(etalon.RewardDescriptionID, reward.RewardDescriptionID);
                });
            await _service.UpdateCompetitionReward(0, null, placeGrant);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CompetitionRewardServiceTests_RemoveCompetitionReward(int etalon)
        {
            _rewardRepo.Setup(x => x.RemoveCompetitionReward(It.IsAny<int>())).Callback<int>((idx) => Assert.Equal(etalon, idx));
            await _service.RemoveCompetitionReward(etalon);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CompetitionRewardServiceTests_GetCompetitionRewards(int etalon)
        {
            List<CompetitionReward> rewards = new List<CompetitionReward>() { new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1), 0) };
            _rewardRepo.Setup(x => x.GetCompetitionRewards(It.IsAny<int>()))
                .Callback<int>((idx) => Assert.Equal(etalon, idx))
                .Returns(Task.FromResult<IEnumerable<CompetitionReward>>(rewards));
            var ret = await _service.GetCompetitionRewards(etalon);
            Assert.Equal(rewards, ret);
        }
    }
}
