using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using RepositoriesRealisation.DatabaseObjects;
using RepositoriesRealisation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample")]
    public class PlayerProfileRepositoryUnitTests : IntegrationTest<IPlayerProfileRepository>
    {
        public PlayerProfileRepositoryUnitTests(ITestOutputHelper helper) : base(helper)
        {
            
        }
        [Fact]
        public async Task GetPlayerProfile_Success()
        {
            await ExecSQLFile("accounts.sql");
            PlayerProfile p = await Testing.GetPlayerProfile(3);
            p.Should().NotBeNull().And.BeEquivalentTo(new PlayerProfile("munny", "munny_description", 3));
        }
        [Fact]
        public async Task GetPlayerProfile_Failure()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetPlayerProfile(5));
        }
        [Fact]
        public async Task UpdatePlayerProfile_Success()
        {
            await ExecSQLFile("accounts.sql");
            PlayerProfile result = new PlayerProfile("munny", "munny_new_description", 3);
            await Testing.UpdatePlayerProfile(result);

            using var context = await GetContext();
            context.PlayerProfiles.Find(3).Should().NotBeNull().And.Satisfy(
                (PlayerProfileModel md) =>
                {
                    md.Description.Should().BeEquivalentTo("munny_new_description", "I updated it!");
                });

            await DoDumpings("updated_description");
        }
        [Fact]
        public async Task UpdatePlayerProfile_Failure()
        {
            await ExecSQLFile("accounts.sql");
            PlayerProfile result = new PlayerProfile("munny", "munny_new_description", 9);
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.UpdatePlayerProfile(result));
        }
        [Fact]
        public async Task GetPlayerProfileImage_Failure()
        {
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetPlayerProfileImage(5));
        }
        [Fact]
        public async Task GetPlayerProfileImage_Success()
        {
            await ExecSQLFile("accounts.sql");
            LargeData data = await Testing.GetPlayerProfileImage(1);
            data.Data.Should().NotBeNull().And.BeEquivalentTo(new byte[] { 0, 1, 2 });
        }
        [Fact]
        public async Task GetPlayerProfileImage_Empty()
        {
            await ExecSQLFile("accounts.sql");
            LargeData data = await Testing.GetPlayerProfileImage(2);
            data.Data.Should().BeEmpty();
        }
        [Fact]
        public async Task UpdatePlayerProfileImage_Failure()
        {
            LargeData dt = new LargeData([4,5,6]);
            await ExecSQLFile("accounts.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.UpdatePlayerProfileImage(5,dt));
        }
        [Fact]
        public async Task UpdatePlayerProfileImage_Success()
        {
            LargeData dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("accounts.sql");
            await Testing.UpdatePlayerProfileImage(2, dt);

            using var context = await GetContext();
            context.AccountsProfileImages.Find(2).Should().NotBeNull().And.Satisfy(
                (AccountModelProfileImage md) =>
                {
                    md.ProfileImage.Should().BeEquivalentTo(dt.Data);
                });

            await DoDumpings("updated_profile_image");
        }
    }
}
