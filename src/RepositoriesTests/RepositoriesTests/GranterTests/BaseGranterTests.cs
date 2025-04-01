using CompetitiveBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using FluentAssertions;
using CompetitiveBackend.Repositories.Exceptions;
namespace RepositoriesTests.RepositoriesTests.GranterTests
{
    public abstract class BaseGranterTests: IntegrationTest<IPlayerRewardRepository>
    {
        private ICompetitionRewardRepository repo = null!;
        protected BaseGranterTests(ITestOutputHelper helper) : base(helper)
        {
        }
        protected sealed override void PostConfiguring(ServiceProvider p)
        {
            repo = p.GetService<ICompetitionRewardRepository>()!;
        }
        protected abstract override void AddMyRepositories(IServiceCollection coll);
        protected abstract string Name { get; }
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 5)]
        [InlineData(2, 4)]
        [InlineData(5, 6)]
        public async Task GrantRewards(int minPlace, int maxPlace)
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await repo?.CreateCompetitionReward(new CompetitionReward(1, 1, "X","X", new PlaceGrantCondition(minPlace, maxPlace)))!;
            int rewardCount = maxPlace - minPlace + 1;
            await Testing.GrantRewardsFor(1);
            using var context = await GetContext();
            context.PlayerReward.ToList().Should().HaveCount(rewardCount);
            await DoDumpings(Name + $"_{minPlace}_{maxPlace}");
        }
        [Fact]
        public async Task CannotGrantTwice()
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await repo?.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 3)))!;

            await Testing.GrantRewardsFor(1);
            await Assert.ThrowsAsync<FailedOperationException>(async() => await Testing.GrantRewardsFor(1));
        }
        [Fact]
        public async Task MissingCompetition()
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.GrantRewardsFor(7));
        }
    }
}
