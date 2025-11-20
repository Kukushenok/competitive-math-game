using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.RewardDescriptionService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class RewardDescriptionServiceTests
    {
        private readonly Mock<IRewardDescriptionRepository> repository;
        private readonly Mock<IImageProcessor> imageProcessor;
        public RewardDescriptionServiceTests()
        {
            repository = new Mock<IRewardDescriptionRepository>();
            imageProcessor = new Mock<IImageProcessor>();
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsSetRewardIcon()
        {
            // Arrange
            var processedData = new LargeData([42]);
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            imageProcessor.Setup(x => x.Process(It.Is<LargeData>(x => x.Data.Length == 3))).ReturnsAsync(processedData);
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            await service.SetRewardIcon(0, new LargeData([1, 2, 3]));

            // Assert
            repository.Verify(x => x.SetRewardIcon(0, processedData), Times.Once);
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsGetRewardIcon()
        {
            // Arrange
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            var etalon = new LargeData([1, 2, 3]);
            repository.Setup(x => x.GetRewardIcon(0)).ReturnsAsync(etalon);
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            LargeData d = await service.GetRewardIcon(0);

            // Assert
            Assert.Equal(etalon.Data, d.Data);
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsGetRewardDescription()
        {
            // Arrange
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            var etalon = new RewardDescription("A", "B", 0);
            repository.Setup(x => x.GetRewardDescription(0)).ReturnsAsync(etalon);
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            RewardDescription r = await service.GetRewardDescription(0);

            // Assert
            Assert.Equal(etalon.Description, r.Description);
            Assert.Equal(etalon.Name, r.Name);
            Assert.Equal(etalon.Id, r.Id);
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsGetAllRewardDescriptions()
        {
            // Arrange
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            List<RewardDescription> etalon = [new RewardDescription("A", "B", 0), new RewardDescription("B", "C", 2)];
            var etalon_d = new DataLimiter(10, 10);
            repository.Setup(x => x.GetAllRewardDescriptions(etalon_d)).ReturnsAsync(etalon);
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            IEnumerable<RewardDescription> r = await service.GetAllRewardDescriptions(etalon_d);

            // Assert
            Assert.Equal(etalon, r);
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsCreateRewardDescription()
        {
            // Arrange
            var d = new RewardDescription("A", "B", 5);
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            repository.Setup(x => x.CreateRewardDescription(d));
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            await service.CreateRewardDescription(d);

            // Assert
            rdValidator.CheckWasCalled();
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsCreateRewardDescriptionFailure()
        {
            // Arrange
            var d = new RewardDescription("A", "B", 5);
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().FailByDefault().Build();
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await service.CreateRewardDescription(d));

            // Assert
            rdValidator.CheckWasCalled();
            repository.Verify(x => x.CreateRewardDescription(It.IsAny<RewardDescription>()), Times.Never);
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsUpdateRewardDescriptionOk()
        {
            // Arrange
            var d = new RewardDescription("A", "B", 5);
            var etalon = new RewardDescription("C", "D", 5);
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>()
                .WithConstraint((x) => WeakRDCheck(x, etalon))
                .Build();
            repository.Setup(x => x.GetRewardDescription(5)).ReturnsAsync(d);
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            await service.UpdateRewardDescription(5, "C", "D");

            // Assert
            rdValidator.CheckWasCalled();
            repository.Verify(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()), Times.Once);
        }

        private static bool WeakRDCheck(RewardDescription a, RewardDescription b)
        {
            return a.Name == b.Name && a.Id == b.Id && a.Description == b.Description;
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsUpdateRewardDescriptionOk2()
        {
            // Arrange
            var d = new RewardDescription("A", "B", 4);
            var etalon = new RewardDescription("C", "B", 4);
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>()
                .WithConstraint((x) => WeakRDCheck(x, etalon))
                .Build();
            repository.Setup(x => x.GetRewardDescription(4)).ReturnsAsync(d);
            repository.Setup(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()));
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            await service.UpdateRewardDescription(4, "C", null);

            // Assert
            rdValidator.CheckWasCalled();
        }

        [Fact]
        public async Task RewardDescriptionServiceTestsUpdateRewardDescriptionFailure()
        {
            // Arrange
            var d = new RewardDescription("A", "B");
            var etalon = new RewardDescription("C", "B", 4);
            MockValidator<RewardDescription> rdValidator = new MockValidatorBuilder<RewardDescription>().FailByDefault().Build();
            repository.Setup(x => x.GetRewardDescription(4)).ReturnsAsync(d);
            var service = new RewardDescriptionService(repository.Object, imageProcessor.Object, rdValidator);

            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await service.UpdateRewardDescription(4, "C", null));

            // Assert
            rdValidator.CheckWasCalled();
            repository.Verify(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()), Times.Never);
        }
    }
}
