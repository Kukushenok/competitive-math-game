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
        private Mock<IPlayerParticipationRepository> _repository;
        private Mock<ICompetitionRepository> _competitionRepo;
        private PlayerParticipationService _service;
        public PlayerParticipationServiceTests()
        {
            _repository = new Mock<IPlayerParticipationRepository>();
            _competitionRepo = new Mock<ICompetitionRepository>();
            _service = new PlayerParticipationService(_repository.Object, _competitionRepo.Object);
        }
        private Competition GetCompetition(int deltaStart, int deltaEnd, int id = 1)
        {
            DateTime now = DateTime.UtcNow;
            return new Competition("a", "b", DateTime.UtcNow + TimeSpan.FromSeconds(deltaStart), DateTime.UtcNow + TimeSpan.FromSeconds(deltaEnd), 1);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_UPDATE()
        {
            // Arrange
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5,10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).ReturnsAsync(new PlayerParticipation(1, 0, 10, DateTime.UtcNow));
            _repository.Setup(x => x.UpdateParticipation(It.Is<PlayerParticipation>(p => 
                (20 == p.Score) && 0 == p.PlayerProfileId && 1 == p.CompetitionId
            )));
                //.Callback<PlayerParticipation>((p) =>
                //{
                //    Assert.Equal(20, p.Score);
                //    Assert.Equal(0, p.PlayerProfileId);
                //    Assert.Equal(1, p.CompetitionId);
                //});
            // Act
            await _service.SubmitParticipation(0, 1, 20);

            // Assert
            _repository.Verify(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()), Times.Once);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_TOO_EARLY()
        {
            // Arrange
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(5, 10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            _repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>())).Throws(new FailedOperationException());

            // Act Assert
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.SubmitParticipation(0, 1, 20));
            _repository.Verify(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()), Times.Never);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_TOO_LATE()
        {
            // Arrange
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-10, -5));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.SubmitParticipation(0, 1, 20));
            // Assert
            _repository.Verify(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()), Times.Never);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_DISCARD()
        {
            // Arrange
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).ReturnsAsync(new PlayerParticipation(1, 0, 10, DateTime.UtcNow));
            _repository.Setup(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()));
            // Act
            await _service.SubmitParticipation(0, 1, 5);
            // Assert
            _repository.Verify(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()), Times.Never);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_CREATE()
        {
            // Arrange
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            _repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()));
                //.Callback<PlayerParticipation>((p) =>
                //{
                //    Assert.Equal(20, p.Score);
                //    Assert.Equal(0, p.PlayerProfileId);
                //    Assert.Equal(1, p.CompetitionId);
                //});
            // Act
            await _service.SubmitParticipation(0, 1, 20);
            // Assert
            _repository.Verify(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()), Times.Once);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_DeleteParticipation()
        {
            // Arrange
            _repository.Setup(x => x.DeleteParticipation(It.IsAny<int>(), It.IsAny<int>()));
            // Act
            await _service.DeleteParticipation(0, 1);
            // Assert
            _repository.Verify(x => x.DeleteParticipation(0, 1), Times.Once);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetParticipation()
        {
            // Arrange
            PlayerParticipation etalon = new PlayerParticipation(1, 0, 10, DateTime.UtcNow);
            _repository.Setup(x => x.GetParticipation(0, 1, It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(etalon);
            // Act
            var r = await _service.GetParticipation(0, 1);
            // Assert
            Assert.Equal(etalon, r);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetLeaderboard()
        {
            // Arrange
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard = new List<PlayerParticipation>()
            {
                new PlayerParticipation(0, 1, 2, DateTime.UtcNow)
            };
            _repository.Setup(x => x.GetLeaderboard(1, It.IsAny<DataLimiter>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((int idx, DataLimiter d, bool _, bool _) => Assert.Equal(etalon_d, d))
                .ReturnsAsync(leaderboard);
            
            // Act
            var r = await _service.GetLeaderboard(1, etalon_d);

            // Assert
            Assert.Equal(leaderboard, r);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetPlayerParticipations()
        {
            // Arrange
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard = new List<PlayerParticipation>()
            {
                new PlayerParticipation(0, 1, 2, DateTime.UtcNow)
            };
            _repository.Setup(x => x.GetPlayerParticipations(0, It.IsAny<DataLimiter>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((int idx, DataLimiter d, bool _, bool _) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<PlayerParticipation>>(leaderboard));
            // Act
            var r = await _service.GetPlayerParticipations(0, etalon_d);

            // Assert
            Assert.Equal(leaderboard, r);
        }
    }
}
