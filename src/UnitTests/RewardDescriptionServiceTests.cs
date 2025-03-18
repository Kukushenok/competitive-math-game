using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Objects;
using CompetitiveBackend.Services.RewardDescriptionService;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class RewardDescriptionServiceTests
    {
        Mock<IRewardDescriptionRepository> _repository;
        Mock<ILargeFileProcessor> _imageProcessor;
        RewardDescriptionService _service;
        public RewardDescriptionServiceTests()
        {
            _repository = new Mock<IRewardDescriptionRepository>();
            _imageProcessor = new Mock<ILargeFileProcessor>();
            _service = new RewardDescriptionService(_repository.Object, _imageProcessor.Object);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_SetRewardIcon()
        {
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
        public async Task RewardDescriptionServiceTests_SetRewardGameAsset()
        {
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.SetRewardGameAsset(0, It.IsAny<LargeData>())).Callback((int a, LargeData d) =>
            {
                Assert.Equal(0, a);
                Assert.Equal(etalon.Data, d.Data);
            });
            _imageProcessor.Setup(x => x.Process(It.IsAny<LargeData>())).ReturnsAsync(new LargeData([42]));
            await _service.SetRewardGameAsset(0, etalon);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardIcon()
        {
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.GetRewardIcon(0)).ReturnsAsync(etalon);
            LargeData d = await _service.GetRewardIcon(0);
            Assert.Equal(etalon.Data, d.Data);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardGameAsset()
        {
            LargeData etalon = new LargeData([1, 2, 3]);
            _repository.Setup(x => x.GetRewardGameAsset(0)).ReturnsAsync(etalon);
            LargeData d = await _service.GetRewardGameAsset(0);
            Assert.Equal(etalon.Data, d.Data);
        }
        [Fact]
        public async Task RewardDescriptionServiceTests_GetRewardDescription()
        {
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
            RewardDescription d = new RewardDescription("A", "B");
            _repository.Setup(x => x.CreateRewardDescription(It.IsAny<RewardDescription>()))
                .Callback<RewardDescription>((rw) =>
                {
                    Assert.Equal(d.Name, rw.Name);
                    Assert.Equal(d.Description, rw.Description);
                    Assert.Equal(d.Id, rw.Id);
                });
            await _service.CreateRewardDescription(d);
        }
    }
}
