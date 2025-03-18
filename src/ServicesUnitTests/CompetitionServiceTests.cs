using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.Exceptions;
using Moq;

namespace ServiceUnitTests
{
    public class CompetitionServiceTests
    {
        private Mock<ICompetitionRepository> _repository;
        private CompetitionService _service;
        public CompetitionServiceTests()
        {
            _repository = new Mock<ICompetitionRepository>();
            _service = new CompetitionService(_repository.Object);
            //_service.GetAllCompetitions();
            //_service.GetActiveCompetitions();
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_OK()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(etalon, c));
            await _service.CreateCompetition(etalon);
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_Bad1()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(etalon, c));
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.CreateCompetition(etalon));
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_Bad2()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(etalon, c));
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.CreateCompetition(etalon));
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_OK()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("a", "b", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(pending, c));

            await _service.UpdateCompetition(0, pending.Name, pending.Description, pending.StartDate, pending.EndDate);
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_OK2()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(pending, c));

            await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate);
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad1()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt - TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(pending, c));

            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad2()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(pending, c));

            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad3()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            Competition pending = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10));
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Equal(pending, c));

            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
        }
        [Fact]
        public async Task CompetitionServiceTest_GetCompetition()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var r = await _service.GetCompetition(0);
            Assert.Equal(etalon, r);
        }

        [Fact]
        public async Task CompetitionServiceTest_GetCompetitionLevel()
        {
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.GetCompetitionLevel(0)).ReturnsAsync(etalon);
            LargeData d = await _service.GetCompetitionLevel(0);
            Assert.Equal(etalon.Data, d.Data);
        }
        [Fact]
        public async Task CompetitionServiceTest_SetCompetitionLevel()
        {
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.SetCompetitionLevel(0, It.IsAny<LargeData>()))
                .Callback<int, LargeData>((idx, d) => Assert.Equal(etalon.Data, d.Data));
            await _service.SetCompetitionLevel(0, etalon);
        }
        [Fact]
        public async Task CompetitionServiceTest_GetAllCompetitions()
        {
            DateTime dt = DateTime.Now;
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<Competition> etalon = new List<Competition>()
            {
                 new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0)
            };
            _repository.Setup(x => x.GetAllCompetitions(It.IsAny<DataLimiter>()))
                .Callback<DataLimiter>((d) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<Competition>>(etalon));
            var r = await _service.GetAllCompetitions(etalon_d);
            Assert.Equal(etalon, r);
        }
        [Fact]
        public async Task CompetitionServiceTest_GetActiveCompetitions()
        {
            DateTime dt = DateTime.Now;
            List<Competition> etalon = new List<Competition>()
            {
                 new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0)
            };
            _repository.Setup(x => x.GetActiveCompetitions())
                .Returns(Task.FromResult<IEnumerable<Competition>>(etalon));
            var r = await _service.GetActiveCompetitions();
            Assert.Equal(etalon, r);
        }
    }
}
