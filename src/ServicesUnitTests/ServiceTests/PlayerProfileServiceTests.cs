using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Exceptions;
using CompetitiveBackend.Services.ExtraTools;
using CompetitiveBackend.Services.PlayerProfileService;
using Moq;

namespace ServicesUnitTests.ServiceTests
{
    public class PlayerProfileServiceTests
    {
        private readonly Mock<IPlayerProfileRepository> profileRepo;
        private readonly Mock<IImageProcessor> fileProcessor;
        private MockValidator<PlayerProfile> validator;
        public PlayerProfileServiceTests()
        {
            profileRepo = new Mock<IPlayerProfileRepository>();
            fileProcessor = new Mock<IImageProcessor>();
        }

        [Fact]
        public async Task PlayerProfileServiceGetPlayerProfile()
        {
            // Arrange
            validator = new MockValidatorBuilder<PlayerProfile>().Build();
            var etalon = new PlayerProfile("P", null, 1);
            profileRepo.Setup(x => x.GetPlayerProfile(0)).ReturnsAsync(etalon);
            var service = new PlayerProfileService(profileRepo.Object, fileProcessor.Object, validator);

            // Act
            PlayerProfile curr = await service.GetPlayerProfile(0);

            // Assert
            Assert.Equal(etalon.Name, curr.Name);
            Assert.Equal(etalon.Description, curr.Description);
            Assert.Equal(etalon.Id, curr.Id);
        }

        [Fact]
        public async Task PlayerProfileServiceUpdatePlayerProfile()
        {
            // Arrange
            var etalon = new PlayerProfile("P", null, 1);
            validator = new MockValidatorBuilder<PlayerProfile>().CheckEtalon(etalon).Build();
            var service = new PlayerProfileService(profileRepo.Object, fileProcessor.Object, validator);

            // Act
            await service.UpdatePlayerProfile(etalon);

            // Assert
            validator.CheckWasCalled();
            profileRepo.Verify(
                x => x.UpdatePlayerProfile(It.Is<PlayerProfile>(curr =>
                (etalon.Name == curr.Name) && (etalon.Description == curr.Description) && (etalon.Id == curr.Id))), Times.Once);
        }

        [Fact]
        public async Task PlayerProfileServiceUpdatePlayerProfileFailure()
        {
            // Arrange
            var etalon = new PlayerProfile("P", null, 1);
            validator = new MockValidatorBuilder<PlayerProfile>().FailByDefault().Build();
            profileRepo.Setup(x => x.UpdatePlayerProfile(It.IsAny<PlayerProfile>())).Callback<PlayerProfile>(
                (curr) =>
                {
                    Assert.Fail("Please do not update DB with incorrect data");
                });
            var service = new PlayerProfileService(profileRepo.Object, fileProcessor.Object, validator);

            // Act
            await Assert.ThrowsAnyAsync<ServiceException>(async () => await service.UpdatePlayerProfile(etalon));

            // Assert
            validator.CheckWasCalled();
        }

        [Fact]
        public async Task PlayerProfileServiceUpdatePlayerProfileImage()
        {
            // Arrange
            validator = new MockValidatorBuilder<PlayerProfile>().Build();
            fileProcessor.Setup(x => x.Process(It.IsAny<LargeData>())).ReturnsAsync(new LargeData([42]));
            var service = new PlayerProfileService(profileRepo.Object, fileProcessor.Object, validator);

            // Act
            await service.SetPlayerProfileImage(0, new LargeData([1, 2, 3]));

            // Assert
            profileRepo.Verify(x => x.UpdatePlayerProfileImage(0, It.Is<LargeData>(p => p.Data.Length == 1 && p.Data[0] == 42)));
        }

        [Fact]
        public async Task PlayerProfileServiceGetPlayerProfileImage()
        {
            // Arrange
            validator = new MockValidatorBuilder<PlayerProfile>().Build();
            profileRepo.Setup(x => x.GetPlayerProfileImage(0)).ReturnsAsync(new LargeData([42]));
            var service = new PlayerProfileService(profileRepo.Object, fileProcessor.Object, validator);

            // Act
            LargeData d = await service.GetPlayerProfileImage(0);

            // Assert
            Assert.Single(d.Data);
            Assert.Equal(42, d.Data[0]);
        }
    }
}
