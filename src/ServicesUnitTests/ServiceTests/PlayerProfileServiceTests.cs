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

            _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object, _validator);

        }
        [Fact]
        public async Task PlayerProfileService_GetPlayerProfile()
        {
            _validator = (new MockValidatorBuilder<PlayerProfile>()).Build();
            PlayerProfile etalon = new PlayerProfile("P", null, 1);
            _profileRepo.Setup(x => x.GetPlayerProfile(0)).ReturnsAsync(etalon);
            PlayerProfile curr = await _service.GetPlayerProfile(0);
            Assert.Equal(etalon.Name, curr.Name);
            Assert.Equal(etalon.Description, curr.Description);
            Assert.Equal(etalon.Id, curr.Id);
        }
        [Fact]
        public async Task PlayerProfileService_UpdatePlayerProfile()
        {
            PlayerProfile etalon = new PlayerProfile("P", null, 1);
            _validator = (new MockValidatorBuilder<PlayerProfile>()).CheckEtalon(etalon).Build();
            _profileRepo.Setup(x => x.UpdatePlayerProfile(It.IsAny<PlayerProfile>())).Callback<PlayerProfile>(
                (curr) =>
                {
                    Assert.Equal(etalon.Name, curr.Name);
                    Assert.Equal(etalon.Description, curr.Description);
                    Assert.Equal(etalon.Id, curr.Id);
                });
            await _service.UpdatePlayerProfile(etalon);
            _validator.CheckWasCalled();
        }
        [Fact]
        public async Task PlayerProfileService_UpdatePlayerProfile_Failure()
        {
            PlayerProfile etalon = new PlayerProfile("P", null, 1);
            _validator = (new MockValidatorBuilder<PlayerProfile>()).FailByDefault().Build();
            _profileRepo.Setup(x => x.UpdatePlayerProfile(It.IsAny<PlayerProfile>())).Callback<PlayerProfile>(
                (curr) =>
                {
                    Assert.Fail("Please do not update DB with incorrect data");
                });
            await Assert.ThrowsAnyAsync<ServiceException>(async () => await _service.UpdatePlayerProfile(etalon));
            _validator.CheckWasCalled();
        }
        [Fact]
        public async Task PlayerProfileService_UpdatePlayerProfileImage()
        {
            _profileRepo.Setup(x => x.UpdatePlayerProfileImage(0, It.IsAny<LargeData>())).Callback((int a, LargeData d) =>
            {
                Assert.Equal(0, a);
                Assert.Single(d.Data);
                Assert.Equal(42, d.Data[0]);
            });
            _fileProcessor.Setup(x => x.Process(It.IsAny<LargeData>())).ReturnsAsync(new LargeData([42]));
            await _service.SetPlayerProfileImage(0, new LargeData([1, 2, 3]));
        }
        [Fact]
        public async Task PlayerProfileService_GetPlayerProfileImage()
        {
            _profileRepo.Setup(x => x.GetPlayerProfileImage(0)).ReturnsAsync(new LargeData([42]));
            LargeData d = await _service.GetPlayerProfileImage(0);
            Assert.Single(d.Data);
            Assert.Equal(42, d.Data[0]);
        }
    }
}
