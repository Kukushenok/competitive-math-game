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
        private readonly CompetitionRewardService service;
        private readonly Mock<ICompetitionRewardRepository> rewardRepo;
        public CompetitionRewardServiceTests()
        {
            rewardRepo = new Mock<ICompetitionRewardRepository>();
            service = new CompetitionRewardService(rewardRepo.Object);
        }

        private static bool CompareWith(CompetitionReward a, CompetitionReward b)
        {
            return a.Name == b.Name &&
                   a.Condition == b.Condition &&
                   a.Description == b.Description &&
                   a.CompetitionID == b.CompetitionID &&
                   a.RewardDescriptionID == b.RewardDescriptionID;
        }

        [Fact]
        public async Task CompetitionRewardServiceTestsCreateCompetitionReward()
        {
            // Arrange
            var etalon = new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1));

            // Act
            await service.CreateCompetitionReward(etalon);

            // Assert
            rewardRepo.Verify(x => x.CreateCompetitionReward(It.Is<CompetitionReward>(x => CompareWith(etalon, x))), Times.Once);
        }

        [Fact]
        public async Task CompetitionRewardServiceTestsUpdateCompetitionReward()
        {
            // Arrange
            var etalon = new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1), 0);
            var placeGrant = new PlaceGrantCondition(1, 1);
            rewardRepo.Setup(x => x.GetCompetitionReward(0)).ReturnsAsync(etalon);

            // Act
            await service.UpdateCompetitionReward(0, null, placeGrant);

            // Assert
            CompetitionReward check = etalon;
            check.Condition = placeGrant;
            rewardRepo.Verify(x => x.UpdateCompetitionReward(It.Is<CompetitionReward>(x => CompareWith(check, x))), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CompetitionRewardServiceTestsRemoveCompetitionReward(int etalon)
        {
            // Act
            await service.RemoveCompetitionReward(etalon);

            // Assert
            rewardRepo.Verify(x => x.RemoveCompetitionReward(It.Is<int>(x => x == etalon)), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CompetitionRewardServiceTestsGetCompetitionRewards(int etalon)
        {
            // Arrange
            List<CompetitionReward> rewards = [new CompetitionReward(1, 2, "3", "4", new RankGrantCondition(0, 1), 0)];
            rewardRepo.Setup(x => x.GetCompetitionRewards(etalon))
                .Returns(Task.FromResult<IEnumerable<CompetitionReward>>(rewards));

            // Act
            IEnumerable<CompetitionReward> ret = await service.GetCompetitionRewards(etalon);

            // Assert
            Assert.Equal(rewards, ret);
        }
    }
}
