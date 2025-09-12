using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Objects;
using CompetitiveBackend.Services.PlayerProfileService;
using ServicesRealisation.ServicesRealisation.Validator;
using Moq;
using System.ComponentModel.DataAnnotations;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;

namespace ServicesUnitTests.ServiceTests
{
    public class PlayerProfileServiceTests
    {
        private PlayerProfileService _service;
        private Mock<IPlayerProfileRepository> _profileRepo;
        private Mock<IImageProcessor> _fileProcessor;
        private MockValidator<PlayerProfile> _validator;
        public PlayerProfileServiceTests()
        {
            _profileRepo = new Mock<IPlayerProfileRepository>();
            _fileProcessor = new Mock<IImageProcessor>();

        }
        [Fact]
        public async Task PlayerProfileService_GetPlayerProfile()
        {
            // Arrange
            _validator = (new MockValidatorBuilder<PlayerProfile>()).Build();
            PlayerProfile etalon = new PlayerProfile("P", null, 1);
            _profileRepo.Setup(x => x.GetPlayerProfile(0)).ReturnsAsync(etalon);
            var _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object, _validator);

            // Act
            PlayerProfile curr = await _service.GetPlayerProfile(0);

            // Assert
            Assert.Equal(etalon.Name, curr.Name);
            Assert.Equal(etalon.Description, curr.Description);
            Assert.Equal(etalon.Id, curr.Id);
        }
        [Fact]
        public async Task PlayerProfileService_UpdatePlayerProfile()
        {
            // Arrange
            PlayerProfile etalon = new PlayerProfile("P", null, 1);
            _validator = (new MockValidatorBuilder<PlayerProfile>()).CheckEtalon(etalon).Build();
            var _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object, _validator);

            // Act
            await _service.UpdatePlayerProfile(etalon);

            // Assert
            _validator.CheckWasCalled();
            _profileRepo.Verify(x => x.UpdatePlayerProfile(It.Is<PlayerProfile>(curr =>
                (etalon.Name == curr.Name) && (etalon.Description == curr.Description) && (etalon.Id == curr.Id)
            )), Times.Once);
        }
        [Fact]
        public async Task PlayerProfileService_UpdatePlayerProfile_Failure()
        {
            // Arrange
            PlayerProfile etalon = new PlayerProfile("P", null, 1);
            _validator = (new MockValidatorBuilder<PlayerProfile>()).FailByDefault().Build();
            _profileRepo.Setup(x => x.UpdatePlayerProfile(It.IsAny<PlayerProfile>())).Callback<PlayerProfile>(
                (curr) =>
                {
                    Assert.Fail("Please do not update DB with incorrect data");
                });
            var _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object, _validator);

            // Act
            await Assert.ThrowsAnyAsync<ServiceException>(async () => await _service.UpdatePlayerProfile(etalon));

            // Assert
            _validator.CheckWasCalled();
        }
        [Fact]
        public async Task PlayerProfileService_UpdatePlayerProfileImage()
        {
            // Arrange
            _validator = (new MockValidatorBuilder<PlayerProfile>()).Build();
            _fileProcessor.Setup(x => x.Process(It.IsAny<LargeData>())).ReturnsAsync(new LargeData([42]));
            var _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object, _validator);

            // Act
            await _service.SetPlayerProfileImage(0, new LargeData([1, 2, 3]));

            // Assert
            _profileRepo.Verify(x => x.UpdatePlayerProfileImage(0, It.Is<LargeData>(p => p.Data.Length == 1 && p.Data[0] == 42)));
        }
        [Fact]
        public async Task PlayerProfileService_GetPlayerProfileImage()
        {
            // Arrange
            _validator = (new MockValidatorBuilder<PlayerProfile>()).Build();
            _profileRepo.Setup(x => x.GetPlayerProfileImage(0)).ReturnsAsync(new LargeData([42]));
            var _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object, _validator);

            // Act
            LargeData d = await _service.GetPlayerProfileImage(0);
            
            // Assert
            Assert.Single(d.Data);
            Assert.Equal(42, d.Data[0]);
        }
    }
}
