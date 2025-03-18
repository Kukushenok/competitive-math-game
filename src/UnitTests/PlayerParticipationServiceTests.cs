using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.PlayerParticipationService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class PlayerParticipationServiceTests
    {
        private Mock<IPlayerParticipationRepository> _repository;
        private PlayerParticipationService _service;
        public PlayerParticipationServiceTests()
        {
            _repository = new Mock<IPlayerParticipationRepository>();
            _service = new PlayerParticipationService(_repository.Object);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_SubmitParticipation_UPDATE()
        {
            _repository.Setup(x => x.GetParticipation(0, 1)).ReturnsAsync(new PlayerParticipation(1, 0, 10));
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
        public async Task PlayerParticipationServiceTests_SubmitParticipation_DISCARD()
        {
            _repository.Setup(x => x.GetParticipation(0, 1)).ReturnsAsync(new PlayerParticipation(1, 0, 10));
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
            _repository.Setup(x => x.GetParticipation(0, 1)).Throws(new MissingDataException());
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
            PlayerParticipation etalon = new PlayerParticipation(1, 0, 10);
            _repository.Setup(x => x.GetParticipation(0, 1)).ReturnsAsync(etalon);
            Assert.Equal(etalon, await _service.GetParticipation(0, 1));
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetLeaderboard()
        {
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard = new List<PlayerParticipation>()
            {
                new PlayerParticipation(0, 1, 2)
            };
            _repository.Setup(x => x.GetLeaderboard(1, It.IsAny<DataLimiter>()))
                .Callback((int idx, DataLimiter d) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<PlayerParticipation>>(leaderboard));
            var r = await _service.GetLeaderboard(1, etalon_d);
            Assert.Equal(leaderboard, r);
        }
        [Fact]
        public async Task PlayerParticipationServiceTests_GetPlayerParticipations()
        {
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<PlayerParticipation> leaderboard = new List<PlayerParticipation>()
            {
                new PlayerParticipation(0, 1, 2)
            };
            _repository.Setup(x => x.GetPlayerParticipations(0, It.IsAny<DataLimiter>()))
                .Callback((int idx, DataLimiter d) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<PlayerParticipation>>(leaderboard));
            var r = await _service.GetPlayerParticipations(0, etalon_d);
            Assert.Equal(leaderboard, r);
        }
    }
}
