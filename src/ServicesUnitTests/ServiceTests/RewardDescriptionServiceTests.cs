using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.Objects;
using CompetitiveBackend.Services.RewardDescriptionService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class RewardDescriptionServiceTests
    {
        Mock<IRewardDescriptionRepository> _repository;
        Mock<IImageProcessor> _imageProcessor;
        public RewardDescriptionServiceTests()
        {
            _repository = new Mock<IRewardDescriptionRepository>();
            _imageProcessor = new Mock<IImageProcessor>();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_SetRewardIcon()
        {
            // Arrange
            var processedData = new LargeData([42]);
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            _imageProcessor.Setup(x => x.Process(It.IsAny<LargeData>())).ReturnsAsync(processedData);
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            await _service.SetRewardIcon(0, new LargeData([1, 2, 3]));
            // Assert
            _repository.Verify(x => x.SetRewardIcon(0, processedData), Times.Once);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardIcon()
        {
            // Arrange
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.GetRewardIcon(0)).ReturnsAsync(etalon);
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            LargeData d = await _service.GetRewardIcon(0);
            // Assert
            Assert.Equal(etalon.Data, d.Data);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardDescription()
        {
            // Arrange
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            RewardDescription etalon = new RewardDescription("A", "B", 0);
            _repository.Setup(x => x.GetRewardDescription(0)).ReturnsAsync(etalon);
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            RewardDescription r = await _service.GetRewardDescription(0);
            // Assert
            Assert.Equal(etalon.Description, r.Description);
            Assert.Equal(etalon.Name, r.Name);
            Assert.Equal(etalon.Id, r.Id);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetAllRewardDescriptions()
        {
            // Arrange
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            List<RewardDescription> etalon = new List<RewardDescription>() { new RewardDescription("A", "B", 0), new RewardDescription("B", "C", 2) };
            DataLimiter etalon_d = new DataLimiter(10, 10);
            _repository.Setup(x => x.GetAllRewardDescriptions(etalon_d)).ReturnsAsync(etalon);
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            var r = await _service.GetAllRewardDescriptions(etalon_d);
            // Assert
            Assert.Equal(etalon, r);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_CreateRewardDescription()
        {
            // Arrange
            RewardDescription d = new RewardDescription("A", "B", 5);
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().Build();
            _repository.Setup(x => x.CreateRewardDescription(d));
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            await _service.CreateRewardDescription(d);
            // Assert
            _rdValidator.CheckWasCalled();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_CreateRewardDescription_Failure()
        {
            // Arrange
            RewardDescription d = new RewardDescription("A", "B", 5);
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().FailByDefault().Build();
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.CreateRewardDescription(d));
            // Assert
            _rdValidator.CheckWasCalled();
            _repository.Verify(x => x.CreateRewardDescription(It.IsAny<RewardDescription>()), Times.Never);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_UpdateRewardDescription_Ok()
        {
            // Arrange
            RewardDescription d = new RewardDescription("A", "B", 5);
            RewardDescription etalon = new RewardDescription("C", "D", 5);
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>()
                .WithConstraint((x) => WeakRDCheck(x, etalon))
                .Build();
            _repository.Setup(x => x.GetRewardDescription(5)).ReturnsAsync(d);
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            await _service.UpdateRewardDescription(5, "C", "D");
            // Assert
            _rdValidator.CheckWasCalled();
            _repository.Verify(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()), Times.Once);
        }
        private bool WeakRDCheck(RewardDescription a, RewardDescription b)
        {
            return a.Name == b.Name && a.Id == b.Id && a.Description == b.Description;
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_UpdateRewardDescription_Ok2()
        {
            // Arrange
            RewardDescription d = new RewardDescription("A", "B", 4);
            RewardDescription etalon = new RewardDescription("C", "B", 4);
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>()
                .WithConstraint((x) => WeakRDCheck(x, etalon))
                .Build();
            _repository.Setup(x => x.GetRewardDescription(4)).ReturnsAsync(d);
            _repository.Setup(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()));
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            await _service.UpdateRewardDescription(4, "C", null);
            // Assert
            _rdValidator.CheckWasCalled();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_UpdateRewardDescription_Failure()
        {
            // Arrange
            RewardDescription d = new RewardDescription("A", "B");
            RewardDescription etalon = new RewardDescription("C", "B", 4);
            MockValidator<RewardDescription> _rdValidator = new MockValidatorBuilder<RewardDescription>().FailByDefault().Build();
            _repository.Setup(x => x.GetRewardDescription(4)).ReturnsAsync(d);
            var _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
            // Act
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateRewardDescription(4, "C", null));
            // Assert
            _rdValidator.CheckWasCalled();
            _repository.Verify(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()), Times.Never);
        }
    }
}
