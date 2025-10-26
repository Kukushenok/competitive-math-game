using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.Exceptions;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionServiceTests
    {
        private readonly Mock<ICompetitionRepository> repository;
        private readonly Mock<ICompetitionRewardScheduler> rewardScheduler;

        public CompetitionServiceTests()
        {
            repository = new Mock<ICompetitionRepository>();
            _ = new MockValidator<Competition>();
            rewardScheduler = new Mock<ICompetitionRewardScheduler>();
        }

        private static Competition CreateEtalon(int deltaSec1, int deltaSec2)
        {
            DateTime dt = DateTime.Now;
            var etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(deltaSec1), dt + TimeSpan.FromSeconds(deltaSec2), 0);
            return etalon;
        }

        private static MockValidator<Competition> GetValidator(Competition comp)
        {
            return new MockValidatorBuilder<Competition>().CheckEtalon(comp).Build();
        }

        private void CheckWasAdded(Competition etalon)
        {
            rewardScheduler.Verify(x => x.OnCompetitionCreated(It.Is<Competition>(x => x.Equals(etalon))), Times.Once);
            repository.Verify(x => x.CreateCompetition(It.Is<Competition>(x => x.Equals(etalon))), Times.Once);
        }

        private void CheckShouldNotAdd()
        {
            repository.Verify(x => x.CreateCompetition(It.IsAny<Competition>()), Times.Never);
            rewardScheduler.Verify(x => x.OnCompetitionCreated(It.IsAny<Competition>()), Times.Never);
        }

        [Fact]
        public async Task CompetitionServiceTestsCreateCompetitionOK()
        {
            // Arrange
            Competition etalon = CreateEtalon(5, 10);
            MockValidator<Competition> validator = GetValidator(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await service.CreateCompetition(etalon);

            // Assert
            CheckWasAdded(etalon);
            validator.CheckWasCalled();
        }

        [Fact]
        public async Task CompetitionServiceTestsCreateCompetitionBad1()
        {
            // Arrange
            Competition etalon = CreateEtalon(-5, 10);
            MockValidator<Competition> validator = GetValidator(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await service.CreateCompetition(etalon));

            // Assert
            validator.CheckWasCalled();
            CheckShouldNotAdd();
        }

        [Fact]
        public async Task CompetitionServiceTestsCreateCompetitionBad2()
        {
            // Arrange
            Competition etalon = CreateEtalon(20, 10);
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().FailByDefault().Build();
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await service.CreateCompetition(etalon));

            // Assert
            validator.CheckWasCalled();
            CheckShouldNotAdd();
        }

        [Fact]
        public async Task CompetitionServiceTestUpdateCompetitionOK()
        {
            // Arrange
            Competition etalon = CreateEtalon(5, 10);
            var pending = new Competition("a", "b", etalon.StartDate, etalon.EndDate, 0);
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await service.UpdateCompetition(0, pending.Name, pending.Description, pending.StartDate, pending.EndDate);

            // Assert
            validator.CheckWasCalled();
            rewardScheduler.Verify(x => x.OnCompetitionUpdated(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
            repository.Verify(x => x.UpdateCompetition(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
        }

        [Fact]
        public async Task CompetitionServiceTestUpdateCompetitionOK2()
        {
            // Arrange
            Competition etalon = CreateEtalon(5, 10);
            Competition pending = etalon;
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate);

            // Assert
            rewardScheduler.Verify(x => x.OnCompetitionUpdated(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
            repository.Verify(x => x.UpdateCompetition(It.Is<Competition>(x => x.Equals(pending))), Times.Once);
            validator.CheckWasCalled();
        }

        [Fact]
        public async Task CompetitionServiceTestUpdateCompetitionBad1()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var etalon = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            var pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt - TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().FailByDefault().Build();
            repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));

            // Assert
            validator.CheckWasCalled();
            CheckShouldNotAdd();
        }

        [Fact]
        public async Task CompetitionServiceTestUpdateCompetitionBad2()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            var pending = new Competition("Hello", "World", dt + TimeSpan.FromSeconds(5), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));

            // Assert
            validator.CheckWasCalled();
            CheckShouldNotAdd();
        }

        [Fact]
        public async Task CompetitionServiceTestUpdateCompetitionBad3()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            var pending = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(20), dt + TimeSpan.FromSeconds(10), 0);
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            await Assert.ThrowsAsync<ChronologicalException>(async () => await service.UpdateCompetition(0, null, null, pending.StartDate, pending.EndDate));

            // Assert
            validator.CheckWasCalled();
            CheckShouldNotAdd();
        }

        [Fact]
        public async Task CompetitionServiceTestGetCompetition()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var etalon = new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0);
            repository.Setup(x => x.GetCompetition(0)).ReturnsAsync(etalon);
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            Competition r = await service.GetCompetition(0);

            // Assert
            Assert.Equal(etalon, r);
        }

        [Fact]
        public async Task CompetitionServiceTestGetAllCompetitions()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            var etalon_d = new DataLimiter(10, 10);
            List<Competition> etalon =
            [
                 new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0),
            ];
            repository.Setup(x => x.GetAllCompetitions(It.IsAny<DataLimiter>()))
                .Callback<DataLimiter>((d) => Assert.Equal(etalon_d, d))
                .Returns(Task.FromResult<IEnumerable<Competition>>(etalon));
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            IEnumerable<Competition> r = await service.GetAllCompetitions(etalon_d);

            // Assert
            Assert.Equal(etalon, r);
        }

        [Fact]
        public async Task CompetitionServiceTestGetActiveCompetitions()
        {
            // Arrange
            DateTime dt = DateTime.Now;
            List<Competition> etalon =
            [
                 new Competition("Hello", "World", dt - TimeSpan.FromSeconds(10), dt - TimeSpan.FromSeconds(5), 0),
            ];
            repository.Setup(x => x.GetActiveCompetitions())
                .Returns(Task.FromResult<IEnumerable<Competition>>(etalon));
            MockValidator<Competition> validator = new MockValidatorBuilder<Competition>().Build();
            var service = new CompetitionService(repository.Object, validator, rewardScheduler.Object);

            // Act
            IEnumerable<Competition> r = await service.GetActiveCompetitions();

            // Assert
            Assert.Equal(etalon, r);
        }
    }
}
