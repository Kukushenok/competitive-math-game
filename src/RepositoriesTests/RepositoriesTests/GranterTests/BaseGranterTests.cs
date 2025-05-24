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
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.Models;
namespace RepositoriesTests.RepositoriesTests.GranterTests
{
    public abstract class BaseGranterTests: IntegrationTest<IPlayerRewardRepository>
    {
        private ICompetitionRewardRepository repo = null!;
        private IPlayerParticipationRepository participations = null!;
        protected BaseGranterTests(ITestOutputHelper helper) : base(helper)
        {
        }
        protected sealed override void PostConfiguring(ServiceProvider p)
        {
            repo = p.GetRequiredService<ICompetitionRewardRepository>();
            participations = p.GetRequiredService<IPlayerParticipationRepository>();
        }
        protected abstract override void AddMyRepositories(IServiceCollection coll);
        protected abstract string Name { get; }
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 5)]
        [InlineData(2, 4)]
        [InlineData(5, 6)]
        public async Task GrantRewardsByPlace(int minPlace, int maxPlace)
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X","X", new PlaceGrantCondition(minPlace, maxPlace)));
            int rewardCount = maxPlace - minPlace + 1;
            await Testing.GrantRewardsFor(1);
            using var context = await GetContext();
            var lst = context.PlayerReward.ToList();
            lst.Should().HaveCount(rewardCount);
            lst.Should().AllSatisfy(x =>
            {
                x.CompetitionID.Should().Be(1, "Competition granted");
                x.RewardDescriptionID.Should().Be(1, "Reward granted");
            });
            await DoDumpings(Name + $"place_{minPlace}_{maxPlace}");
        }
        [Theory]
        [InlineData(1.0, 1.0)]
        [InlineData(0.5, 1.0)]
        [InlineData(0.0, 1.0)]
        [InlineData(0.25, 0.75)]
        public async Task GrantRewardsByRank(float minRank, float maxRank)
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new RankGrantCondition(minRank, maxRank)));

            await Testing.GrantRewardsFor(1);
            using var context = await GetContext();
            int GRANT_COUNT = await context.PlayerParticipation.Where(x => x.CompetitionID == 1).CountAsync();
            int minPlace = (int)Math.Floor(GRANT_COUNT * (1.0 - maxRank)) + 1;
            int maxPlace = (int)Math.Ceiling(GRANT_COUNT * (1.0 - minRank)) + 1;
            if (maxPlace >= GRANT_COUNT) maxPlace = GRANT_COUNT;
            int rewardCount = maxPlace - minPlace + 1;
            var lst = context.PlayerReward.ToList();
            lst.Should().HaveCount(rewardCount);
            lst.Should().AllSatisfy(x =>
            {
                x.CompetitionID.Should().Be(1, "Competition granted");
                x.RewardDescriptionID.Should().Be(1, "Reward granted");
            });
            await DoDumpings(Name + $"rank_{minRank:f2}_{maxRank:f2}");
        }
        public static object[][] Arguments = [
            [new GrantCondition[] {new PlaceGrantCondition(1, 1), new PlaceGrantCondition(2, 2)}],
            [new GrantCondition[] {new PlaceGrantCondition(1, 1), new RankGrantCondition(0.5f, 1.0f)}],
            [new GrantCondition[] {new RankGrantCondition(0.25f, 0.75f), new RankGrantCondition(0.5f, 1.0f)}],
            [new GrantCondition[] {new RankGrantCondition(0.0f, 0.0f), new PlaceGrantCondition(1, 1)}]
            ];
        [Theory]
        [MemberData(nameof(Arguments))]
        public async Task GetMultipleRewardData(GrantCondition[] conditions)
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            using var context = await GetContext();
            int PLAYER_COUNT = await context.PlayerParticipation.Where(x => x.CompetitionID == 1).CountAsync();
            int rewardCount = 0;
            foreach(GrantCondition cnd in conditions)
            {
                await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", cnd));
                if (cnd is PlaceGrantCondition row)
                {
                    rewardCount += row.maxPlace - row.minPlace + 1;
                }
                else if(cnd is RankGrantCondition rank)
                {
                    int minPlace = (int)Math.Floor(PLAYER_COUNT * (1.0 - rank.maxRank)) + 1;
                    int maxPlace = (int)Math.Ceiling(PLAYER_COUNT * (1.0 - rank.minRank)) + 1;
                    if (maxPlace >= PLAYER_COUNT) maxPlace = PLAYER_COUNT;
                    rewardCount += maxPlace - minPlace + 1;
                }
            }
            await Testing.GrantRewardsFor(1);
            var lst = context.PlayerReward.ToList();
            lst.Should().HaveCount(rewardCount);
        }
        [Fact]
        public async Task CannotGrantTwice()
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 3)));

            await Testing.GrantRewardsFor(1);
            await Assert.ThrowsAsync<FailedOperationException>(async() => await Testing.GrantRewardsFor(1));
        }
        [Fact]
        public async Task MissingCompetition()
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.GrantRewardsFor(7));
        }
        [Fact]
        public async Task GrantRewardsWithoutParticipants()
        {
            await ExecSQLFile("to_grant_no_participants.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 1)));
            await Testing.GrantRewardsFor(1);
            using var context = await GetContext();
            (await context.PlayerReward.ToListAsync()).Should().HaveCount(0);
        }
        [Fact]
        public async Task GrantRewardsTwoParticipants()
        {
            await ExecSQLFile("to_grant_no_participants.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 1)));
            await repo.CreateCompetitionReward(new CompetitionReward(2, 1, "X", "X", new PlaceGrantCondition(2, 2)));
            DateTime dt = DateTime.UtcNow;
            await participations.CreateParticipation(new PlayerParticipation(1, 1, 200, dt));
            await participations.CreateParticipation(new PlayerParticipation(1, 2, 150, dt - TimeSpan.FromSeconds(1)));
            await Testing.GrantRewardsFor(1);
            using var context = await GetContext();
            List<PlayerRewardModel> cd = (await context.PlayerReward.ToListAsync());
            cd.Should().HaveCount(2);
            cd.Find(x => x.PlayerID == 1)!.RewardDescriptionID.Should().Be(1);
            cd.Find(x => x.PlayerID == 2)!.RewardDescriptionID.Should().Be(2);
        }
        [Fact]
        public async Task GrantRewardsTwoParticipantsWithSameScore()
        {
            await ExecSQLFile("to_grant_no_participants.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 1)));
            await repo.CreateCompetitionReward(new CompetitionReward(2, 1, "X", "X", new PlaceGrantCondition(2, 2)));
            DateTime dt = DateTime.UtcNow;
            await participations.CreateParticipation(new PlayerParticipation(1, 1, 200, dt - TimeSpan.FromSeconds(1)));
            await participations.CreateParticipation(new PlayerParticipation(1, 2, 200, dt));
            await Testing.GrantRewardsFor(1);
            using var context = await GetContext();
            List<PlayerRewardModel> cd = (await context.PlayerReward.ToListAsync());
            cd.Should().HaveCount(2);
            cd.Find(x => x.PlayerID == 1)!.RewardDescriptionID.Should().Be(1);
            cd.Find(x => x.PlayerID == 2)!.RewardDescriptionID.Should().Be(2);
        }
    }
}
