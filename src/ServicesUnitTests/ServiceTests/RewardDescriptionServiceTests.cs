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
        MockValidator<RewardDescription> _rdValidator;
        RewardDescriptionService _service;
        public RewardDescriptionServiceTests()
        {
            _repository = new Mock<IRewardDescriptionRepository>();
            _imageProcessor = new Mock<IImageProcessor>();
            _rdValidator = new MockValidator<RewardDescription>();
            _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object, _rdValidator);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_SetRewardIcon()
        {
            _rdValidator.Reset();
            _repository.Setup(x => x.SetRewardIcon(0, It.IsAny<LargeData>())).Callback((int a, LargeData d) =>
            {
                Assert.Equal(0, a);
                Assert.Single(d.Data);
                Assert.Equal(42, d.Data[0]);
            });
            _imageProcessor.Setup(x => x.Process(It.IsAny<LargeData>())).ReturnsAsync(new LargeData([42]));
            await _service.SetRewardIcon(0, new LargeData([1, 2, 3]));
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardIcon()
        {
            _rdValidator.Reset();
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.GetRewardIcon(0)).ReturnsAsync(etalon);
            LargeData d = await _service.GetRewardIcon(0);
            Assert.Equal(etalon.Data, d.Data);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardDescription()
        {
            _rdValidator.Reset();
            RewardDescription etalon = new RewardDescription("A", "B", 0);
            _repository.Setup(x => x.GetRewardDescription(0)).ReturnsAsync(etalon);
            RewardDescription r = await _service.GetRewardDescription(0);
            Assert.Equal(etalon.Description, r.Description);
            Assert.Equal(etalon.Name, r.Name);
            Assert.Equal(etalon.Id, r.Id);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetAllRewardDescriptions()
        {
            _rdValidator.Reset();
            List<RewardDescription> etalon = new List<RewardDescription>() { new RewardDescription("A", "B", 0), new RewardDescription("B", "C", 2) };
            DataLimiter etalon_d = new DataLimiter(10, 10);
            _repository.Setup(x => x.GetAllRewardDescriptions(It.IsAny<DataLimiter>()))
                .Callback<DataLimiter>((d) => Assert.Equal(etalon_d, d)).ReturnsAsync(etalon);
            var r = await _service.GetAllRewardDescriptions(etalon_d);
            Assert.Equal(etalon, r);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_CreateRewardDescription()
        {
            RewardDescription d = new RewardDescription("A", "B", 5);
            _rdValidator.Reset(d);
            _repository.Setup(x => x.CreateRewardDescription(It.IsAny<RewardDescription>()))
                .Callback<RewardDescription>((rw) =>
                {
                    Assert.Equal(d.Name, rw.Name);
                    Assert.Equal(d.Description, rw.Description);
                    Assert.Equal(d.Id, rw.Id);
                });
            await _service.CreateRewardDescription(d);
            _rdValidator.Check();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_CreateRewardDescription_Failure()
        {
            RewardDescription d = new RewardDescription("A", "B", 5);
            _rdValidator.Reset(d, true);
            _repository.Setup(x => x.CreateRewardDescription(It.IsAny<RewardDescription>()))
                .Callback<RewardDescription>((rw) =>
                {
                    Assert.Fail("Please do not update DB with incorrect data");
                });
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.CreateRewardDescription(d));
            _rdValidator.Check();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_UpdateRewardDescription_Ok()
        {
            RewardDescription d = new RewardDescription("A", "B", 5);
            RewardDescription etalon = new RewardDescription("C", "D", 5);
            _rdValidator.Reset(etalon);
            _repository.Setup(x => x.GetRewardDescription(5)).ReturnsAsync(d);
            _repository.Setup(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()))
                        .Callback<RewardDescription>((rw) =>
                        {
                            Assert.Equal(etalon.Name, rw.Name);
                            Assert.Equal(etalon.Description, rw.Description);
                            Assert.Equal(etalon.Id, rw.Id);
                        });
            await _service.UpdateRewardDescription(5, "C", "D");
            _rdValidator.Check();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_UpdateRewardDescription_Ok2()
        {
            RewardDescription d = new RewardDescription("A", "B", 4);
            RewardDescription etalon = new RewardDescription("C", "B", 4);
            _rdValidator.Reset(etalon);
            _repository.Setup(x => x.GetRewardDescription(4)).ReturnsAsync(d);
            _repository.Setup(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()))
                        .Callback<RewardDescription>((rw) =>
                        {
                            Assert.Equal(etalon.Name, rw.Name);
                            Assert.Equal(etalon.Description, rw.Description);
                            Assert.Equal(etalon.Id, rw.Id);
                        });
            await _service.UpdateRewardDescription(4, "C", null);
            _rdValidator.Check();
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_UpdateRewardDescription_Failure()
        {
            RewardDescription d = new RewardDescription("A", "B");
            RewardDescription etalon = new RewardDescription("C", "B", 4);
            _rdValidator.Reset(etalon, true);
            _repository.Setup(x => x.GetRewardDescription(4)).ReturnsAsync(d);
            _repository.Setup(x => x.UpdateRewardDescription(It.IsAny<RewardDescription>()))
                        .Callback<RewardDescription>((rw) =>
                        {
                            Assert.Fail("Please do not update DB with incorrect data");
                        });
            await Assert.ThrowsAsync<InvalidArgumentsException>(async () => await _service.UpdateRewardDescription(4, "C", null));
            _rdValidator.Check();
        }
    }
}
