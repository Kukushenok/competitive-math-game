using AwesomeAssertions;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    public class RewardDescriptionRepositoryTests : IntegrationTest<IRewardDescriptionRepository>
    {
        public RewardDescriptionRepositoryTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task CreateRewardDescriptionSuccess()
        {
            var dscr = new RewardDescription("Hello", "There");
            var rd = new RewardDescriptionModel(dscr.Name, dscr.Description);
            await testing.CreateRewardDescription(dscr);
            using RepositoriesRealisation.BaseDbContext dbContext = await GetContext();
            dbContext.RewardDescription.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(
                rd,
                options => options.Excluding(x => x.Id));

            await DoDumpings("create_one");
        }

        [Fact]
        public async Task UpdateRewardDescriptionFailure()
        {
            await ExecSQLFile("reward_descriptions.sql");
            var dscr = new RewardDescription("Hello!", "Hai!", 8);
            await Assert.ThrowsAsync<MissingDataException>(async () => await testing.UpdateRewardDescription(dscr));
        }

        [Fact]
        public async Task UpdateRewardDescriptionSuccess()
        {
            await ExecSQLFile("reward_descriptions.sql");
            var dscr = new RewardDescription("Hello!", "Hai!", 1);
            var rd = new RewardDescriptionModel(1, dscr.Name, dscr.Description);
            await testing.UpdateRewardDescription(dscr);

            using RepositoriesRealisation.BaseDbContext dbContext = await GetContext();
            dbContext.RewardDescription.Find(1).Should().BeEquivalentTo(rd);
        }

        [Fact]
        public async Task GetRewardIconFailure()
        {
            await ExecSQLFile("reward_descriptions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await testing.GetRewardIcon(5));
        }

        [Fact]
        public async Task GetRewardIconSuccess()
        {
            await ExecSQLFile("reward_descriptions.sql");
            LargeData data = await testing.GetRewardIcon(2);
            data.Data.Should().NotBeNull().And.BeEquivalentTo(new byte[] { 0, 1, 2 });
        }

        [Fact]
        public async Task GetRewardIconEmpty()
        {
            await ExecSQLFile("reward_descriptions.sql");
            LargeData data = await testing.GetRewardIcon(1);
            data.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task SetRewardIconFailure()
        {
            var dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("reward_descriptions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await testing.SetRewardIcon(5, dt));
        }

        [Fact]
        public async Task SetRewardIconSuccess()
        {
            var dt = new LargeData([4, 5, 6]);
            await ExecSQLFile("reward_descriptions.sql");
            await testing.SetRewardIcon(2, dt);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.RewardDescriptionIconImages.Find(2).Should().NotBeNull().And.Satisfy(
                (RewardDescriptionModelIconImage md) =>
                {
                    md.IconImage.Should().BeEquivalentTo(dt.Data);
                });

            await DoDumpings("set_reward_icon");
        }

        // [Fact]
        // public async Task GetGameAsset_Failure()
        // {
        //    await ExecSQLFile("reward_descriptions.sql");
        //    await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetRewardGameAsset(5));
        // }
        // [Fact]
        // public async Task GetGameAsset_Success()
        // {
        //    await ExecSQLFile("reward_descriptions.sql");
        //    LargeData data = await Testing.GetRewardGameAsset(2);
        //    data.Data.Should().NotBeNull().And.BeEquivalentTo(new byte[] { 3, 4, 5 });
        // }
        // [Fact]
        // public async Task GetGameAsset_Empty()
        // {
        //    await ExecSQLFile("reward_descriptions.sql");
        //    LargeData data = await Testing.GetRewardGameAsset(1);
        //    data.Data.Should().BeEmpty();
        // }
        // [Fact]
        // public async Task SetGameAsset_Failure()
        // {
        //    LargeData dt = new LargeData([4, 5, 6]);
        //    await ExecSQLFile("reward_descriptions.sql");
        //    await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.SetRewardGameAsset(5, dt));
        // }
        // [Fact]
        // public async Task SetGameAsset_Success()
        // {
        //    LargeData dt = new LargeData([4, 8, 6]);
        //    await ExecSQLFile("reward_descriptions.sql");
        //    await Testing.SetRewardGameAsset(2, dt);

        // using var context = await GetContext();
        //    context.RewardDescriptionInGameData.Find(2).Should().NotBeNull().And.Satisfy(
        //        (RewardDescriptionModelInGameData md) =>
        //        {
        //            md.InGameData.Should().BeEquivalentTo(dt.Data);
        //        });

        // await DoDumpings("set_reward_asset");
        // }
    }
}
