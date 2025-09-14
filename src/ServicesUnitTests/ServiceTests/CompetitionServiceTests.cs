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
        private Mock<ICompetitionRewardScheduler> _rewardScheduler;

        public CompetitionServiceTests()
        {
            _repository = new Mock<ICompetitionRepository>();
            MockValidator<Competition> _validator = new MockValidator<Competition>();
            _rewardScheduler = new Mock<ICompetitionRewardScheduler>();
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_OK()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).CheckEtalon(etalon).Build();
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await _service.CreateCompetition(etalon);
            
            // Assert
            _rewardScheduler.Verify(x => x.OnCompetitionCreated(It.Is<Competition>(x => x.Equals(etalon))), Times.Once);
            _repository.Verify(x => x.CreateCompetition(It.Is<Competition>(x => x.Equals(etalon))), Times.Once);
            _validator.CheckWasCalled();
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_Bad1()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10));
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).CheckEtalon(etalon).Build();
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.CreateCompetition(etalon));
            
            // Assert
            _validator.CheckWasCalled();
            _repository.Verify(x => x.CreateCompetition(It.IsAny<Competition>()), Times.Never);
            _rewardScheduler.Verify(x => x.OnCompetitionCreated(It.IsAny<Competition>()), Times.Never);
        }
        [Fact]
        public async Task CompetitionServiceTests_CreateCompetition_Bad2()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10));
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).FailByDefault().Build();
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.CreateCompetition(etalon));
            
            // Assert
            _validator.CheckWasCalled();
            _repository.Verify(x => x.CreateCompetition(It.IsAny<Competition>()), Times.Never);
            _rewardScheduler.Verify(x => x.OnCompetitionCreated(It.IsAny<Competition>()), Times.Never);
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_OK()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("a", "b", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).Build();
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await _service.UpdateCompetition(0, pending.Name, pending.Description, pending.StartDate, pending.EndDate);
            
            // Assert
            _validator.CheckWasCalled();
            _rewardScheduler.Verify(x => x.OnCompetitionUpdated(It.Is<Competition>(x=>x.Equals(pending))), Times.Once);
            _repository.Verify(x => x.UpdateCompetition(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_OK2()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).Build();
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate);
            
            // Assert
            _rewardScheduler.Verify(x => x.OnCompetitionUpdated(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
            _repository.Verify(x => x.UpdateCompetition(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
            _validator.CheckWasCalled();
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad1()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt - TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).FailByDefault().Build();
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
            
            // Assert
            _validator.CheckWasCalled();
            _repository.Verify(x => x.CreateCompetition(It.IsAny<Competition>()), Times.Never);
            _rewardScheduler.Verify(x => x.OnCompetitionCreated(It.IsAny<Competition>()), Times.Never);
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad2()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            Competition pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).Build();
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
            
            // Assert
            _validator.CheckWasCalled();
            _repository.Verify(x => x.CreateCompetition(It.IsAny<Competition>()), Times.Never);
            _rewardScheduler.Verify(x => x.OnCompetitionCreated(It.IsAny<Competition>()), Times.Never);
        }
        [Fact]
        public async Task CompetitionServiceTest_UpdateCompetition_Bad3()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            Competition pending = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> _validator = (new MockValidatorBuilder<Competition>()).Build();
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await _service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));
            
            // Assert
            _validator.CheckWasCalled();
            _repository.Verify(x => x.CreateCompetition(It.IsAny<Competition>()), Times.Never);
            _rewardScheduler.Verify(x => x.OnCompetitionCreated(It.IsAny<Competition>()), Times.Never);
        }
        [Fact]
        public async Task CompetitionServiceTest_GetCompetition()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            Competition etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            _repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var _validator = new MockValidatorBuilder<Competition>().Build();
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            var r = await _service.GetCompetition(0);
            
            // Assert
            Assert.Equal(etalon, r);
        }
        [Fact]
        public async Task CompetitionServiceTest_GetAllCompetitions()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            DataLimiter etalon_d = new DataLimiter(10, 10);
            List<Competition> etalon = new List<Competition>()
            {
                 new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0)
            };
            _repository.Setup(x => x.GetAllCompetitions(It.IsAny<DataLimiter>()))
                .Callback<DataLimiter>((d) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<Competition>>(etalon));
            var _validator = new MockValidatorBuilder<Competition>().Build();
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            var r = await _service.GetAllCompetitions(etalon_d);
            
            // Assert
            Assert.Equal(etalon, r);
        }
        [Fact]
        public async Task CompetitionServiceTest_GetActiveCompetitions()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            List<Competition> etalon = new List<Competition>()
            {
                 new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0)
            };
            _repository.Setup(x => x.GetActiveCompetitions())
                .Returns(Task.FromResult<IEnumerable<Competition>>(etalon));
            var _validator = new MockValidatorBuilder<Competition>().Build();
            var _service = new CompetitionService(_repository.Object, _validator, _rewardScheduler.Object);
            
            // Act
            var r = await _service.GetActiveCompetitions();
            
            // Assert
            Assert.Equal(etalon, r);
        }
    }
}
