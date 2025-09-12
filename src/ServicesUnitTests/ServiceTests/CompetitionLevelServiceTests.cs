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
            // Arrange
            var levelDataInfo = new LevelDataInfo(1, 2, "Platform", 123);
            var largeData = new LargeData(new byte[10]);
            _mockRepo.Setup(r => r.AddCompetitionLevel(largeData, levelDataInfo))
                    .Returns(Task.CompletedTask);

            // Act
            await _service.CreateLevelData(levelDataInfo, largeData);

            // Assert
            _mockRepo.Verify(r => r.AddCompetitionLevel(largeData, levelDataInfo), Times.Once);
        }

        [Fact]
        public async Task GetAllLevelData_CallsRepositoryWithCorrectId()
        {
            // Arrange
            const int competitionId = 5;
            var expected = new List<LevelDataInfo>();
            _mockRepo.Setup(r => r.GetAllLevelData(competitionId))
                    .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllLevelData(competitionId);

            // Assert
            _mockRepo.Verify(r => r.GetAllLevelData(competitionId), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCompetitionLevel_CallsRepositoryWithParameters()
        {
            // Arrange
            const int competitionId = 1;
            const string platform = "Android";
            const int maxVersion = 3;
            var expected = new LargeData(Array.Empty<byte>());
            _mockRepo.Setup(r => r.GetCompetitionLevel(competitionId, platform, maxVersion))
                    .ReturnsAsync(expected);

            // Act
            var result = await _service.GetCompetitionLevel(competitionId, platform, maxVersion);

            // Assert
            _mockRepo.Verify(r => r.GetCompetitionLevel(competitionId, platform, maxVersion), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCompetitionLevel_WithoutOptionalParameters_UsesNulls()
        {
            // Arrange
            const int competitionId = 1;
            var expected = new LargeData(Array.Empty<byte>());
            _mockRepo.Setup(r => r.GetCompetitionLevel(competitionId, null, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _service.GetCompetitionLevel(competitionId);

            // Assert
            _mockRepo.Verify(r => r.GetCompetitionLevel(competitionId, null, null), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task UpdateCompetitionLevelData_CallsRepositoryWithParameters()
        {
            // Arrange
            const int levelDataId = 123;
            var largeData = new LargeData(new byte[20]);
            _mockRepo.Setup(r => r.UpdateCompetitionLevelData(levelDataId, largeData))
                    .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateCompetitionLevelData(levelDataId, largeData);

            // Assert
            _mockRepo.Verify(r => r.UpdateCompetitionLevelData(levelDataId, largeData), Times.Once);
        }

        [Fact]
        public async Task UpdateCompetitionLevelInfo_CallsRepositoryWithData()
        {
            // Arrange
            var levelDataInfo = new LevelDataInfo(1, 2, "Platform", 123);
            _mockRepo.Setup(r => r.UpdateCompetitionLevelInfo(levelDataInfo))
                    .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateCompetitionLevelInfo(levelDataInfo);

            // Assert
            _mockRepo.Verify(r => r.UpdateCompetitionLevelInfo(levelDataInfo), Times.Once);
        }

        [Fact]
        public async Task GetCompetitionLevelInfo_CallsRepositoryWithData()
        {
            // Arrange
            var excepted = new LevelDataInfo(1, 2, "Platform", 123);
            _mockRepo.Setup(r => r.GetSpecificCompetitionLevelInfo(123))
                    .ReturnsAsync(excepted);

            // Act
            await _service.GetSpecificCompetitionLevelInfo(123);

            // Assert
            _mockRepo.Verify(r => r.GetSpecificCompetitionLevelInfo(123), Times.Once);
        }
        [Fact]
        public async Task GetCompetitionLevelData_CallsRepositoryWithData()
        {
            // Arrange
            var expected = new LargeData(Array.Empty<byte>());
            _mockRepo.Setup(r => r.GetSpecificCompetitionLevel(123))
                    .ReturnsAsync(expected);

            // Act
            await _service.GetSpecificCompetitionLevel(123);

            // Assert
            _mockRepo.Verify(r => r.GetSpecificCompetitionLevel(123), Times.Once);
        }
        [Fact]
        public async Task DeleteLevelData_CallsRepositoryWithCorrectId()
        {
            // Arrange
            const int levelDataId = 456;
            _mockRepo.Setup(r => r.DeleteCompetitionLevel(levelDataId))
                    .Returns(Task.CompletedTask);

            // Act
            await _service.DeleteLevelData(levelDataId);

            // Assert
            _mockRepo.Verify(r => r.DeleteCompetitionLevel(levelDataId), Times.Once);
        }
    }
}