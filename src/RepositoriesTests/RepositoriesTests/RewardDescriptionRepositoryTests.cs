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
    public class RewardDescriptionRepositoryTests : IntegrationTest<IRewardDescriptionRepository>
    {
        public RewardDescriptionRepositoryTests(ITestOutputHelper helper) : base(helper)
        {
        }
        [Fact]
        public async Task CreateRewardDescription_Success()
        {
            RewardDescription dscr = new RewardDescription("Hello", "There");
            RewardDescriptionModel rd = new RewardDescriptionModel(dscr.Name, dscr.Description);
            await Testing.CreateRewardDescription(dscr);
            using var dbContext = await GetContext();
            dbContext.RewardDescription.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(rd,
                options => options.Excluding(x => x.Id));

            await DoDumpings("create_one");
        }
        [Fact]
        public async Task UpdateRewardDescription_Failure()
        {
            await ExecSQLFile("reward_descriptions.sql");
            RewardDescription dscr = new RewardDescription("Hello!", "Hai!", 8);
            await Assert.ThrowsAsync<MissingDataException>(async()=>await Testing.UpdateRewardDescription(dscr));
        }
        [Fact]
        public async Task UpdateRewardDescription_Success()
        {
            await ExecSQLFile("reward_descriptions.sql");
            RewardDescription dscr = new RewardDescription("Hello!", "Hai!", 1);
            RewardDescriptionModel rd = new RewardDescriptionModel(1, dscr.Name, dscr.Description);
            await Testing.UpdateRewardDescription(dscr);


            using var dbContext = await GetContext();
            dbContext.RewardDescription.Find(1).Should().BeEquivalentTo(rd);
        }
        [Fact]
        public async Task GetRewardIcon_Failure()
        {
            await ExecSQLFile("reward_descriptions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetRewardIcon(5));
        }
        [Fact]
        public async Task GetRewardIcon_Success()
        {
            await ExecSQLFile("reward_descriptions.sql");
            LargeData data = await Testing.GetRewardIcon(2);
            data.Data.Should().NotBeNull().And.BeEquivalentTo(new byte[] { 0, 1, 2 });
        }
        [Fact]
        public async Task GetRewardIcon_Empty()
        {
            await ExecSQLFile("reward_descriptions.sql");
            LargeData data = await Testing.GetRewardIcon(1);
            data.Data.Should().BeEmpty();
        }
        [Fact]
        public async Task SetRewardIcon_Failure()
        {
            LargeData dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("reward_descriptions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.SetRewardIcon(5, dt));
        }
        [Fact]
        public async Task SetRewardIcon_Success()
        {
            LargeData dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("reward_descriptions.sql");
            await Testing.SetRewardIcon(2, dt);

            using var context = await GetContext();
            context.RewardDescriptionIconImages.Find(2).Should().NotBeNull().And.Satisfy(
                (RewardDescriptionModelIconImage md) =>
                {
                    md.IconImage.Should().BeEquivalentTo(dt.Data);
                });

            await DoDumpings("set_reward_icon");
        }

        [Fact]
        public async Task GetGameAsset_Failure()
        {
            await ExecSQLFile("reward_descriptions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetRewardGameAsset(5));
        }
        [Fact]
        public async Task GetGameAsset_Success()
        {
            await ExecSQLFile("reward_descriptions.sql");
            LargeData data = await Testing.GetRewardGameAsset(2);
            data.Data.Should().NotBeNull().And.BeEquivalentTo(new byte[] { 3, 4, 5 });
        }
        [Fact]
        public async Task GetGameAsset_Empty()
        {
            await ExecSQLFile("reward_descriptions.sql");
            LargeData data = await Testing.GetRewardGameAsset(1);
            data.Data.Should().BeEmpty();
        }
        [Fact]
        public async Task SetGameAsset_Failure()
        {
            LargeData dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("reward_descriptions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.SetRewardGameAsset(5, dt));
        }
        [Fact]
        public async Task SetGameAsset_Success()
        {
            LargeData dt = new LargeData([4, 8, 6]);
            await ExecSQLFile("reward_descriptions.sql");
            await Testing.SetRewardGameAsset(2, dt);

            using var context = await GetContext();
            context.RewardDescriptionInGameData.Find(2).Should().NotBeNull().And.Satisfy(
                (RewardDescriptionModelInGameData md) =>
                {
                    md.InGameData.Should().BeEquivalentTo(dt.Data);
                });

            await DoDumpings("set_reward_asset");
        }
    }
}
