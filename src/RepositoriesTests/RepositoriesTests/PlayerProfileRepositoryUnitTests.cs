using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample")]
    public class PlayerProfileRepositoryUnitTests : IntegrationTest<IPlayerProfileRepository>
    {
        public PlayerProfileRepositoryUnitTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task GetPlayerProfileSuccess()
        {
            await ExecSQLFile("accounts.sql");
            PlayerProfile p = await testing.GetPlayerProfile(3);
            p.Should().NotBeNull().And.BeEquivalentTo(new PlayerProfile("munny", "munny_description", 3));
        }

        [Fact]
        public async Task GetPlayerProfileFailure()
        {
            await ExecSQLFile("accounts.sql");
            await (async () => await testing.GetPlayerProfile(5)).Should().ThrowExactlyAsync<MissingDataException>();
        }

        [Fact]
        public async Task UpdatePlayerProfileSuccess()
        {
            await ExecSQLFile("accounts.sql");
            var result = new PlayerProfile("munny", "munny_new_description", 3);
            await testing.UpdatePlayerProfile(result);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.PlayerProfiles.Find(3).Should().NotBeNull().And.Satisfy(
                (PlayerProfileModel md) =>
                {
                    md.Description.Should().BeEquivalentTo("munny_new_description", "I updated it!");
                });

            await DoDumpings("updated_description");
        }

        [Fact]
        public async Task UpdatePlayerProfileFailure()
        {
            await ExecSQLFile("accounts.sql");
            var result = new PlayerProfile("munny", "munny_new_description", 9);
            await (async () => await testing.UpdatePlayerProfile(result)).Should().ThrowExactlyAsync<MissingDataException>();
        }

        [Fact]
        public async Task GetPlayerProfileImageFailure()
        {
            await ExecSQLFile("accounts.sql");
            await (async () => await testing.GetPlayerProfileImage(5)).Should().ThrowExactlyAsync<MissingDataException>();
        }

        [Fact]
        public async Task GetPlayerProfileImageSuccess()
        {
            await ExecSQLFile("accounts.sql");
            LargeData data = await testing.GetPlayerProfileImage(1);
            data.Data.Should().NotBeNull().And.BeEquivalentTo(new byte[] { 0, 1, 2 });
        }

        [Fact]
        public async Task GetPlayerProfileImageEmpty()
        {
            await ExecSQLFile("accounts.sql");
            LargeData data = await testing.GetPlayerProfileImage(2);
            data.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdatePlayerProfileImageFailure()
        {
            var dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("accounts.sql");
            await (async () => await testing.UpdatePlayerProfileImage(5, dt)).Should().ThrowExactlyAsync<MissingDataException>();
        }

        [Fact]
        public async Task UpdatePlayerProfileImageSuccess()
        {
            var dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("accounts.sql");
            await testing.UpdatePlayerProfileImage(2, dt);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.AccountsProfileImages.Find(2).Should().NotBeNull().And.Satisfy(
                (AccountModelProfileImage md) =>
                {
                    md.ProfileImage.Should().BeEquivalentTo(dt.Data);
                });

            await DoDumpings("updated_profile_image");
        }
    }
}
