using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.Objects;
using CompetitiveBackend.Services.PlayerProfileService;
using Moq;

namespace ServiceUnitTests
{
    public class PlayerProfileServiceTests
    {
        private PlayerProfileService _service;
        private Mock<IPlayerProfileRepository> _profileRepo;
        private Mock<ILargeFileProcessor> _fileProcessor;
        public PlayerProfileServiceTests()
        {
            _profileRepo = new Mock<IPlayerProfileRepository>();
            _fileProcessor = new Mock<ILargeFileProcessor>();
            _service = new PlayerProfileService(_profileRepo.Object, _fileProcessor.Object);

        }
        [Fact]
        public async Task PlayerProfileService_GetPlayerProfile()
        {
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
            _profileRepo.Setup(x => x.UpdatePlayerProfile(It.IsAny<PlayerProfile>())).Callback<PlayerProfile>(
                (curr) =>
                {
                    Assert.Equal(etalon.Name, curr.Name);
                    Assert.Equal(etalon.Description, curr.Description);
                    Assert.Equal(etalon.Id, curr.Id);
                });
            await _service.UpdatePlayerProfile(etalon);
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
