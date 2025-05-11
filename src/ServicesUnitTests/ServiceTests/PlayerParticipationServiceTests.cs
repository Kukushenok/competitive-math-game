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
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5,10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).ReturnsAsync(new PlayerParticipation(1, 0, 10, DateTime.UtcNow));
            _repository.Setup(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()))
                .Callback<PlayerParticipation>((p) =>
                {
                    Assert.Equal(20, p.Score);
                    Assert.Equal(0, p.PlayerProfileId);
                    Assert.Equal(1, p.CompetitionId);
                });
            await _service.SubmitParticipation(0, 1, 20);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_TOO_EARLY()
        {
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(5, 10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            _repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()))
                .Callback<PlayerParticipation>((p) =>
                {
                    Assert.Fail("No no no!");
                });
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.SubmitParticipation(0, 1, 20));
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_TOO_LATE()
        {
            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-10, -5));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            _repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()))
                .Callback<PlayerParticipation>((p) =>
                {
                    Assert.Fail("No no no!");
                });
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.SubmitParticipation(0, 1, 20));
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_DISCARD()
        {

            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).ReturnsAsync(new PlayerParticipation(1, 0, 10, DateTime.UtcNow));
            _repository.Setup(x => x.UpdateParticipation(It.IsAny<PlayerParticipation>()))
                .Callback<PlayerParticipation>((p) =>
                {
                    Assert.Fail();
                });
            await _service.SubmitParticipation(0, 1, 5);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_CREATE()
        {

            _competitionRepo.Setup(x => x.GetCompetition(1)).ReturnsAsync(GetCompetition(-5, 10));
            _repository.Setup(x => x.GetParticipation(0, 1, false, false)).Throws(new MissingDataException());
            _repository.Setup(x => x.CreateParticipation(It.IsAny<PlayerParticipation>()))
                .Callback<PlayerParticipation>((p) =>
                {
                    Assert.Equal(20, p.Score);
                    Assert.Equal(0, p.PlayerProfileId);
                    Assert.Equal(1, p.CompetitionId);
                });
            await _service.SubmitParticipation(0, 1, 20);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_DeleteParticipation()
        {
            _repository.Setup(x => x.DeleteParticipation(It.IsAny<int>(), It.IsAny<int>())).Callback((int player, int comp) =>
            {
                Assert.Equal(0, player);
                Assert.Equal(1, comp);
            });
            await _service.DeleteParticipation(0, 1);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetParticipation()
        {
            PlayerParticipation etalon = new PlayerParticipation(1, 0, 10, DateTime.UtcNow);
            _repository.Setup(x => x.GetParticipation(0, 1, It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(etalon);
            Assert.Equal(etalon, await _service.GetParticipation(0, 1));
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetLeaderboard()
        {
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard = new List<PlayerParticipation>()
            {
                new PlayerParticipation(0, 1, 2, DateTime.UtcNow)
            };
            _repository.Setup(x => x.GetLeaderboard(1, It.IsAny<DataLimiter>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((int idx, DataLimiter d, bool _, bool _) => Assert.Equal(etalon_d, d))
                .ReturnsAsync(leaderboard);
            var r = await _service.GetLeaderboard(1, etalon_d);
            Assert.Equal(leaderboard, r);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetPlayerParticipations()
        {
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard = new List<PlayerParticipation>()
            {
                new PlayerParticipation(0, 1, 2, DateTime.UtcNow)
            };
            _repository.Setup(x => x.GetPlayerParticipations(0, It.IsAny<DataLimiter>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Callback((int idx, DataLimiter d, bool _, bool _) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<PlayerParticipation>>(leaderboard));
            var r = await _service.GetPlayerParticipations(0, etalon_d);
            Assert.Equal(leaderboard, r);
        }
    }
}
