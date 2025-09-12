using Allure.Xunit.Attributes;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionRewardService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    // Todo
    public class CompetitionRewardServiceTests
    {
        private CompetitionRewardService _service;
        private Mock<ICompetitionRewardRepository> _rewardRepo;
        public CompetitionRewardServiceTests()
        {
            _rewardRepo = new Mock<ICompetitionRewardRepository>();
            _service = new CompetitionRewardService(_rewardRepo.Object);
        }
        private bool CompareWith(CompetitionReward a, CompetitionReward b)
        {
            return a.Name == b.Name &&
                   a.Condition == b.Condition &&
                   a.Description == b.Description &&
                   a.CompetitionID == b.CompetitionID &&
                   a.RewardDescriptionID == b.RewardDescriptionID;
        }
        [Fact]
        public async Task CompetitionRewardServiceTests_CreateCompetitionReward()
        {
            // Arrange
            CompetitionReward etalon = new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1));

            // Act
            await _service.CreateCompetitionReward(etalon);

            // Assert
            _rewardRepo.Verify(x => x.CreateCompetitionReward(It.Is<CompetitionReward>(x => CompareWith(etalon, x))), Times.Once);
        }
        [Fact]
        public async Task CompetitionRewardServiceTests_UpdateCompetitionReward()
        {
            // Arrange
            CompetitionReward etalon = new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1), 0);
            var placeGrant = new PlaceGrantCondition(1, 1);
            _rewardRepo.Setup(x => x.GetCompetitionReward(0)).ReturnsAsync(etalon);

            // Act
            await _service.UpdateCompetitionReward(0, null, placeGrant);

            // Assert
            CompetitionReward check = etalon;
            check.Condition = placeGrant;
            _rewardRepo.Verify(x => x.UpdateCompetitionReward(It.Is<CompetitionReward>(x => CompareWith(check, x))), Times.Once);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CompetitionRewardServiceTests_RemoveCompetitionReward(int etalon)
        {
            // Act
            await _service.RemoveCompetitionReward(etalon);

            // Assert
            _rewardRepo.Verify(x => x.RemoveCompetitionReward(It.Is<int>(x => x == etalon)), Times.Once);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CompetitionRewardServiceTests_GetCompetitionRewards(int etalon)
        {
            // Arrange
            List<CompetitionReward> rewards = new List<CompetitionReward>() { new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1), 0) };
            _rewardRepo.Setup(x => x.GetCompetitionRewards(etalon))
                .Returns(Task.FromResult<IEnumerable<CompetitionReward>>(rewards));

            // Act
            var ret = await _service.GetCompetitionRewards(etalon);

            // Assert
            Assert.Equal(rewards, ret);
        }
    }
}
