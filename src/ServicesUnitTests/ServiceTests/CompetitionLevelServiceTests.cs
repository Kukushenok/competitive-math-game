using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.ServicesRealisation;
using Moq;
namespace ServicesUnitTests.ServiceTests
{
    public class CompetitionLevelServiceTests
    {
        [Obsolete]
        private readonly Mock<ICompetitionLevelRepository> mockRepo;
        [Obsolete]
        private readonly CompetitionLevelService service;

        [Obsolete]
        public CompetitionLevelServiceTests()
        {
            mockRepo = new Mock<ICompetitionLevelRepository>();
            service = new CompetitionLevelService(mockRepo.Object);
        }

        [Fact]
        [Obsolete]
        public async Task CreateLevelDataCallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var levelDataInfo = new LevelDataInfo(1, 2, "Platform", 123);
            var largeData = new LargeData(new byte[10]);
            mockRepo.Setup(r => r.AddCompetitionLevel(largeData, levelDataInfo))
                    .Returns(Task.CompletedTask);

            // Act
            await service.CreateLevelData(levelDataInfo, largeData);

            // Assert
            mockRepo.Verify(r => r.AddCompetitionLevel(largeData, levelDataInfo), Times.Once);
        }

        [Fact]
        [Obsolete]
        public async Task GetAllLevelDataCallsRepositoryWithCorrectId()
        {
            // Arrange
            const int competitionId = 5;
            var expected = new List<LevelDataInfo>();
            mockRepo.Setup(r => r.GetAllLevelData(competitionId))
                    .ReturnsAsync(expected);

            // Act
            IEnumerable<LevelDataInfo> result = await service.GetAllLevelData(competitionId);

            // Assert
            mockRepo.Verify(r => r.GetAllLevelData(competitionId), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCompetitionLevelCallsRepositoryWithParameters()
        {
            // Arrange
            const int competitionId = 1;
            const string platform = "Android";
            const int maxVersion = 3;
            var expected = new LargeData([]);
            mockRepo.Setup(r => r.GetCompetitionLevel(competitionId, platform, maxVersion))
                    .ReturnsAsync(expected);

            // Act
            LargeData result = await service.GetCompetitionLevel(competitionId, platform, maxVersion);

            // Assert
            mockRepo.Verify(r => r.GetCompetitionLevel(competitionId, platform, maxVersion), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetCompetitionLevelWithoutOptionalParametersUsesNulls()
        {
            // Arrange
            const int competitionId = 1;
            var expected = new LargeData([]);
            mockRepo.Setup(r => r.GetCompetitionLevel(competitionId, null, null))
                    .ReturnsAsync(expected);

            // Act
            LargeData result = await service.GetCompetitionLevel(competitionId);

            // Assert
            mockRepo.Verify(r => r.GetCompetitionLevel(competitionId, null, null), Times.Once);
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task UpdateCompetitionLevelDataCallsRepositoryWithParameters()
        {
            // Arrange
            const int levelDataId = 123;
            var largeData = new LargeData(new byte[20]);
            mockRepo.Setup(r => r.UpdateCompetitionLevelData(levelDataId, largeData))
                    .Returns(Task.CompletedTask);

            // Act
            await service.UpdateCompetitionLevelData(levelDataId, largeData);

            // Assert
            mockRepo.Verify(r => r.UpdateCompetitionLevelData(levelDataId, largeData), Times.Once);
        }

        [Fact]
        [Obsolete]
        public async Task UpdateCompetitionLevelInfoCallsRepositoryWithData()
        {
            // Arrange
            var levelDataInfo = new LevelDataInfo(1, 2, "Platform", 123);
            mockRepo.Setup(r => r.UpdateCompetitionLevelInfo(levelDataInfo))
                    .Returns(Task.CompletedTask);

            // Act
            await service.UpdateCompetitionLevelInfo(levelDataInfo);

            // Assert
            mockRepo.Verify(r => r.UpdateCompetitionLevelInfo(levelDataInfo), Times.Once);
        }

        [Fact]
        [Obsolete]
        public async Task GetCompetitionLevelInfoCallsRepositoryWithData()
        {
            // Arrange
            var excepted = new LevelDataInfo(1, 2, "Platform", 123);
            mockRepo.Setup(r => r.GetSpecificCompetitionLevelInfo(123))
                    .ReturnsAsync(excepted);

            // Act
            await service.GetSpecificCompetitionLevelInfo(123);

            // Assert
            mockRepo.Verify(r => r.GetSpecificCompetitionLevelInfo(123), Times.Once);
        }

        [Fact]
        public async Task GetCompetitionLevelDataCallsRepositoryWithData()
        {
            // Arrange
            var expected = new LargeData([]);
            mockRepo.Setup(r => r.GetSpecificCompetitionLevel(123))
                    .ReturnsAsync(expected);

            // Act
            await service.GetSpecificCompetitionLevel(123);

            // Assert
            mockRepo.Verify(r => r.GetSpecificCompetitionLevel(123), Times.Once);
        }

        [Fact]
        public async Task DeleteLevelDataCallsRepositoryWithCorrectId()
        {
            // Arrange
            const int levelDataId = 456;
            mockRepo.Setup(r => r.DeleteCompetitionLevel(levelDataId))
                    .Returns(Task.CompletedTask);

            // Act
            await service.DeleteLevelData(levelDataId);

            // Assert
            mockRepo.Verify(r => r.DeleteCompetitionLevel(levelDataId), Times.Once);
        }
    }
}