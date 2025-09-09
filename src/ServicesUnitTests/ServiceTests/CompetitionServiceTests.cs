using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.Exceptions;
using Moq;
using ServicesRealisation.ServicesRealisation.Validator;
using System.ComponentModel.DataAnnotations;

namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionServiceTests
    {
        private Mock<ICompetitionRepository> _repository;
        private MockValidator<Competition> _validator;
        private Mock<ICompetitionRewardScheduler> _rewardScheduler;
        private CompetitionService _service;
        public CompetitionServiceTests()
        {
            _repository = new Mock<ICompetitionRepository>();
            _validator = new MockValidator<Competition>();
            _rewardScheduler = new Mock<ICompetitionRewardScheduler>();
            _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);

            //_service.GetAllCompetitions();
            //_service.GetActiveCompetitions();
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_OK()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            _validator.Reset(etalon);
            int calls = 0;
            _rewardScheduler.Setup(x=>x.OnCompetitionCreated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Equal(etalon, c); calls++; });
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => {Assert.Equal(etalon, c); calls++; });
            await _service.CreateCompetition(etalon);
            Assert.Equal(2, calls);
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_Bad1()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            _validator.Reset(etalon);
            _rewardScheduler.Setup(x => x.OnCompetitionCreated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Fail("Invalid data was added"); });
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Fail("Invalid data was added"));
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.CreateCompetition(etalon));
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_Bad2()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10));
            _validator.Reset(etalon, true);
            _rewardScheduler.Setup(x => x.OnCompetitionCreated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Fail("Invalid data was added"); });
            _repository.Setup(x => x.CreateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Fail("Invalid data was added"));
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.CreateCompetition(etalon));
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_OK()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("a", "b", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            _validator.Reset(pending);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            int calls = 0;
            _rewardScheduler.Setup(x => x.OnCompetitionUpdated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Equal(pending, c); calls++; });
            _repository.Setup(x => x.UpdateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Equal(pending, c); calls++; });

            await _service.UpdateCompetition(0, pending.Name, pending.Description, pending.StartDate, pending.EndDate);
            Assert.Equal(2, calls);
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_OK2()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            _validator.Reset(pending);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            int calls = 0;
            _rewardScheduler.Setup(x => x.OnCompetitionUpdated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Equal(pending, c); calls++; });
            _repository.Setup(x => x.UpdateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Equal(pending, c); calls++; });
            await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate);
            Assert.Equal(2, calls);
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad1()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);

            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt - TimeSpan.FromSeconds(10), 0);
            _validator.Reset(pending, true);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _rewardScheduler.Setup(x => x.OnCompetitionUpdated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Fail("Invalid data was added"); });
            _repository.Setup(x => x.UpdateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Fail("Invalid data was added"));

            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad2()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            _validator.Reset(pending);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _rewardScheduler.Setup(x => x.OnCompetitionUpdated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Fail("Invalid data was added"); });
            _repository.Setup(x => x.UpdateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Fail("Invalid data was added"));

            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
            _validator.Check();
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad3()
        {
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            Competition pending = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10), 0);
            _validator.Reset(pending);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            _rewardScheduler.Setup(x => x.OnCompetitionUpdated(It.IsAny<Competition>())).Callback<Competition>((c) => { Assert.Fail("Invalid data was added"); });
            _repository.Setup(x => x.UpdateCompetition(It.IsAny<Competition>())).Callback<Competition>((c) => Assert.Fail("Invalid data was added"));

            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
            _validator.Check();
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
