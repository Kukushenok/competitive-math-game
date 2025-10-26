using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.PlayerParticipationService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class PlayerParticipationServiceTests
    {
        private readonly Mock<IPlayerParticipationRepository> repository;
        private readonly PlayerParticipationService service;
        public PlayerParticipationServiceTests()
        {
            repository = new Mock<IPlayerParticipationRepository>();
            service = new PlayerParticipationService(repository.Object);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsDeleteParticipation()
        {
            // Arrange
            repository.Setup(x => x.DeleteParticipation(It.IsAny<int>(), It.IsAny<int>()));

            // Act
            await service.DeleteParticipation(0, 1);

            // Assert
            repository.Verify(x => x.DeleteParticipation(0, 1), Times.Once);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsGetParticipation()
        {
            // Arrange
            var etalon = new PlayerParticipation(1, 0, 10, DateTime.UtcNow);
            repository.Setup(x => x.GetParticipation(0, 1, It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(etalon);

            // Act
            PlayerParticipation r = await service.GetParticipation(0, 1);

            // Assert
            Assert.Equal(etalon, r);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsGetLeaderboard()
        {
            // Arrange
            var etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard =
            [
                new PlayerParticipation(0, 1, 2, DateTime.UtcNow),
            ];
            repository.Setup(x => x.GetLeaderboard(1, It.IsAny<DataLimiter>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((int idx, DataLimiter d, bool _, bool _) => Assert.Equal(etalon_d, d))
                .ReturnsAsync(leaderboard);

            // Act
            IEnumerable<PlayerParticipation> r = await service.GetLeaderboard(1, etalon_d);

            // Assert
            Assert.Equal(leaderboard, r);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsGetPlayerParticipations()
        {
            // Arrange
            var etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard =
            [
                new PlayerParticipation(0, 1, 2, DateTime.UtcNow),
            ];
            repository.Setup(x => x.GetPlayerParticipations(0, It.IsAny<DataLimiter>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((int idx, DataLimiter d, bool _, bool _) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<PlayerParticipation>>(leaderboard));

            // Act
            IEnumerable<PlayerParticipation> r = await service.GetPlayerParticipations(0, etalon_d);

            // Assert
            Assert.Equal(leaderboard, r);
        }
    }
}
