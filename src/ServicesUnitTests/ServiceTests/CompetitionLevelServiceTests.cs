using Moq;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ServicesRealisation;
using CompetitiveBackend.Core.Objects;
namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionLevelServiceTests
    {
        private readonly Mock<ICompetitionLevelRepository> _mockRepo;
        private readonly CompetitionLevelService _service;

        public CompetitionLevelServiceTests()
        {
            _mockRepo = new Mock<ICompetitionLevelRepository>();
            _service = new CompetitionLevelService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateLevelData_CallsRepositoryWithCorrectParameters()
        {
            var levelDataInfo = new LevelDataInfo(1, 2, "Platform", 123);
            var largeData = new LargeData(new byte[10]);

            _mockRepo.Setup(r => r.AddCompetitionLevel(It.IsAny<LargeData>(), It.IsAny<LevelDataInfo>()))
                    .Returns(Task.CompletedTask);
            await _service.CreateLevelData(levelDataInfo, largeData);
            _mockRepo.Verify(r => r.AddCompetitionLevel(largeData, levelDataInfo), Times.Once);
        }

        [Fact]
        public async Task GetAllLevelData_CallsRepositoryWithCorrectId()
        {
            const int competitionId = 5;
            var expected = new List<LevelDataInfo>();
            _mockRepo.Setup(r => r.GetAllLevelData(competitionId))
                    .ReturnsAsync(expected);
            var result = await _service.GetAllLevelData(competitionId);
            _mockRepo.Verify(r => r.GetAllLevelData(competitionId), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCompetitionLevel_CallsRepositoryWithParameters()
        {
            const int competitionId = 1;
            const string platform = "Android";
            const int maxVersion = 3;
            var expected = new LargeData(Array.Empty<byte>());
            _mockRepo.Setup(r => r.GetCompetitionLevel(competitionId, platform, maxVersion))
                    .ReturnsAsync(expected);
            var result = await _service.GetCompetitionLevel(competitionId, platform, maxVersion);
            _mockRepo.Verify(r => r.GetCompetitionLevel(competitionId, platform, maxVersion), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCompetitionLevel_WithoutOptionalParameters_UsesNulls()
        {
            const int competitionId = 1;
            var expected = new LargeData(Array.Empty<byte>());
            _mockRepo.Setup(r => r.GetCompetitionLevel(competitionId, null, null))
                    .ReturnsAsync(expected);
            var result = await _service.GetCompetitionLevel(competitionId);
            _mockRepo.Verify(r => r.GetCompetitionLevel(competitionId, null, null), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task UpdateCompetitionLevelData_CallsRepositoryWithParameters()
        {
            const int levelDataId = 123;
            var largeData = new LargeData(new byte[20]);
            _mockRepo.Setup(r => r.UpdateCompetitionLevelData(levelDataId, largeData))
                    .Returns(Task.CompletedTask);
            await _service.UpdateCompetitionLevelData(levelDataId, largeData);
            _mockRepo.Verify(r => r.UpdateCompetitionLevelData(levelDataId, largeData), Times.Once);
        }

        [Fact]
        public async Task UpdateCompetitionLevelInfo_CallsRepositoryWithData()
        {
            var levelDataInfo = new LevelDataInfo(1, 2, "Platform", 123);
            _mockRepo.Setup(r => r.UpdateCompetitionLevelInfo(levelDataInfo))
                    .Returns(Task.CompletedTask);
            await _service.UpdateCompetitionLevelInfo(levelDataInfo);
            _mockRepo.Verify(r => r.UpdateCompetitionLevelInfo(levelDataInfo), Times.Once);
        }

        [Fact]
        public async Task GetCompetitionLevelInfo_CallsRepositoryWithData()
        {
            var excepted = new LevelDataInfo(1, 2, "Platform", 123);
            _mockRepo.Setup(r => r.GetSpecificCompetitionLevelInfo(123))
                    .ReturnsAsync(excepted);
            await _service.GetSpecificCompetitionLevelInfo(123);
            _mockRepo.Verify(r => r.GetSpecificCompetitionLevelInfo(123), Times.Once);
        }
        [Fact]
        public async Task GetCompetitionLevelData_CallsRepositoryWithData()
        {
            var expected = new LargeData(Array.Empty<byte>());
            _mockRepo.Setup(r => r.GetSpecificCompetitionLevel(123))
                    .ReturnsAsync(expected);
            await _service.GetSpecificCompetitionLevel(123);
            _mockRepo.Verify(r => r.GetSpecificCompetitionLevel(123), Times.Once);
        }
        [Fact]
        public async Task DeleteLevelData_CallsRepositoryWithCorrectId()
        {
            const int levelDataId = 456;
            _mockRepo.Setup(r => r.DeleteCompetitionLevel(levelDataId))
                    .Returns(Task.CompletedTask);
            await _service.DeleteLevelData(levelDataId);
            _mockRepo.Verify(r => r.DeleteCompetitionLevel(levelDataId), Times.Once);
        }
    }
}