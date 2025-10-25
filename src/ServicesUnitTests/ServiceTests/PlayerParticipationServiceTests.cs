using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.PlayerParticipationService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class PlayerParticipationServiceTests
    {
        private readonly Mock<IPlayerParticipationRepository> repository;
        private readonly Mock<ICompetitionRepository> competitionRepo;
        private readonly PlayerParticipationService service;
        public PlayerParticipationServiceTests()
        {
            repository = new Mock<IPlayerParticipationRepository>();
            competitionRepo = new Mock<ICompetitionRepository>();
            service = new PlayerParticipationService(repository.Object, competitionRepo.Object);
        }

        private static Competition GetCompetition(int deltaStart, int deltaEnd, int id = 1)
        {
            _ = DateTime.UtcNow;
            return new Competition("a", "b", DateTime.UtcNow + TimeSpan.FromSeconds(deltaStart), DateTime.UtcNow + TimeSpan.FromSeconds(deltaEnd), 1);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsSubmitParticipationUPDATE()
        {
            // Arrange
            competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            repository.Setup(x => x.GetParticipation(0, 1, false, false)).ReturnsAsync(new PlayerParticipation(1, 0, 10, DateTime.UtcNow));
            repository.Setup(x => x.UpdateParticipation(It.Is<PlayerParticipation>(p =>
                (p.Score == 20) && p.PlayerProfileId == 0 && p.CompetitionId == 1)));

            // .Callback<PlayerParticipation>((p) =>
            // {
            //    Assert.Equal(20, p.Score);
            //    Assert.Equal(0, p.PlayerProfileId);
            //    Assert.Equal(1, p.CompetitionId);
            // });
            // Act
            await service.SubmitParticipation(0, 1, 20);

            // Assert
            repository.Verify(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()), Times.Once);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsSubmitParticipationTOOEARLY()
        {
            // Arrange
            competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(5, 10));
            repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>())).Throws(new FailedOperationException());

            // Act Assert
            await Assert.ThrowsAsync<ChronologicalException>(async () => await service.SubmitParticipation(0, 1, 20));
            repository.Verify(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()), Times.Never);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsSubmitParticipationTOOLATE()
        {
            // Arrange
            competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-10, -5));
            repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());

            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await service.SubmitParticipation(0, 1, 20));

            // Assert
            repository.Verify(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()), Times.Never);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsSubmitParticipationDISCARD()
        {
            // Arrange
            competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            repository.Setup(x => x.GetParticipation(0, 1, false, false)).ReturnsAsync(new PlayerParticipation(1, 0, 10, DateTime.UtcNow));
            repository.Setup(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()));

            // Act
            await service.SubmitParticipation(0, 1, 5);

            // Assert
            repository.Verify(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()), Times.Never);
        }

        [Fact]
        public async Task PlayerParticipationServiceTestsSubmitParticipationCREATE()
        {
            // Arrange
            competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()));

            // .Callback<PlayerParticipation>((p) =>
            // {
            //    Assert.Equal(20, p.Score);
            //    Assert.Equal(0, p.PlayerProfileId);
            //    Assert.Equal(1, p.CompetitionId);
            // });
            // Act
            await service.SubmitParticipation(0, 1, 20);

            // Assert
            repository.Verify(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()), Times.Once);
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
