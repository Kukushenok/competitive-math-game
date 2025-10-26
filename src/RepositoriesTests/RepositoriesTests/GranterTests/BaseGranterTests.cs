using AwesomeAssertions;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;
namespace RepositoriesTests.RepositoriesTests.GranterTests
{
    public abstract class BaseGranterTests : IntegrationTest<IPlayerRewardRepository>
    {
        private ICompetitionRewardRepository repo = null!;
        private IPlayerParticipationRepository participations = null!;
        protected BaseGranterTests(ITestOutputHelper helper)
            : base(helper)
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
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(minPlace, maxPlace)));
            int rewardCount = maxPlace - minPlace + 1;
            await testing.GrantRewardsFor(1);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
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

            await testing.GrantRewardsFor(1);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            int gRANT_COUNT = await context.PlayerParticipation.Where(x => x.CompetitionID == 1).CountAsync();
            int minPlace = (int)Math.Floor(gRANT_COUNT * (1.0 - maxRank)) + 1;
            int maxPlace = (int)Math.Ceiling(gRANT_COUNT * (1.0 - minRank)) + 1;
            if (maxPlace >= gRANT_COUNT)
            {
                maxPlace = gRANT_COUNT;
            }

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

        public static readonly object[][] Arguments = [
            [new GrantCondition[] { new PlaceGrantCondition(1, 1), new PlaceGrantCondition(2, 2) }],
            [new GrantCondition[] { new PlaceGrantCondition(1, 1), new RankGrantCondition(0.5f, 1.0f) }],
            [new GrantCondition[] { new RankGrantCondition(0.25f, 0.75f), new RankGrantCondition(0.5f, 1.0f) }],
            [new GrantCondition[] { new RankGrantCondition(0.0f, 0.0f), new PlaceGrantCondition(1, 1) }]
            ];
        [Theory]
        [MemberData(nameof(Arguments))]
        public async Task GetMultipleRewardData(GrantCondition[] conditions)
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            int pLAYER_COUNT = await context.PlayerParticipation.Where(x => x.CompetitionID == 1).CountAsync();
            int rewardCount = 0;
            foreach (GrantCondition cnd in conditions)
            {
                await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", cnd));
                if (cnd is PlaceGrantCondition row)
                {
                    rewardCount += row.MaxPlace - row.MinPlace + 1;
                }
                else if (cnd is RankGrantCondition rank)
                {
                    int minPlace = (int)Math.Floor(pLAYER_COUNT * (1.0 - rank.MaxRank)) + 1;
                    int maxPlace = (int)Math.Ceiling(pLAYER_COUNT * (1.0 - rank.MinRank)) + 1;
                    if (maxPlace >= pLAYER_COUNT)
                    {
                        maxPlace = pLAYER_COUNT;
                    }

                    rewardCount += maxPlace - minPlace + 1;
                }
            }

            await testing.GrantRewardsFor(1);
            var lst = context.PlayerReward.ToList();
            lst.Should().HaveCount(rewardCount);
        }

        [Fact]
        public async Task CannotGrantTwice()
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 3)));

            await testing.GrantRewardsFor(1);
            await Assert.ThrowsAsync<FailedOperationException>(async () => await testing.GrantRewardsFor(1));
        }

        [Fact]
        public async Task MissingCompetition()
        {
            await ExecSQLFile("faked_data_to_grant.sql");
            await Assert.ThrowsAsync<RepositoryException>(async () => await testing.GrantRewardsFor(7));
        }

        [Fact]
        public async Task GrantRewardsWithoutParticipants()
        {
            await ExecSQLFile("to_grant_no_participants.sql");
            await repo.CreateCompetitionReward(new CompetitionReward(1, 1, "X", "X", new PlaceGrantCondition(1, 1)));
            await testing.GrantRewardsFor(1);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            (await context.PlayerReward.ToListAsync()).Should().BeEmpty();
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
            await testing.GrantRewardsFor(1);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            List<PlayerRewardModel> cd = await context.PlayerReward.ToListAsync();
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
            await testing.GrantRewardsFor(1);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            List<PlayerRewardModel> cd = await context.PlayerReward.ToListAsync();
            cd.Should().HaveCount(2);
            cd.Find(x => x.PlayerID == 1)!.RewardDescriptionID.Should().Be(1);
            cd.Find(x => x.PlayerID == 2)!.RewardDescriptionID.Should().Be(2);
        }
    }
}
