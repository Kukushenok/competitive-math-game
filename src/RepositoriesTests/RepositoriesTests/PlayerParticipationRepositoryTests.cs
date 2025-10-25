using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    public class PlayerParticipationRepositoryTests : IntegrationTest<IPlayerParticipationRepository>
    {
        public PlayerParticipationRepositoryTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task CreateParticipationTest()
        {
            await ExecSQLFile("participations.sql");
            var exp = new PlayerParticipationModel(1, 6, 100, DateTime.UtcNow);
            await testing.CreateParticipation(new PlayerParticipation(exp.CompetitionID, exp.AccountID, exp.Score, exp.LastUpdateTime));
            using RepositoriesRealisation.BaseDbContext dbContext = await GetContext();
            dbContext.PlayerParticipation.Find(6, 1).Should().NotBeNull().And.BeEquivalentTo(exp, (a) => a.Excluding(f => f.LastUpdateTime));
        }

        [Fact]
        public async Task CreateParticipationTestAlreadyExist()
        {
            await ExecSQLFile("participations.sql");
            var exp = new PlayerParticipationModel(1, 5, 100, DateTime.UtcNow);
            await (async () => await testing.CreateParticipation(new PlayerParticipation(exp.CompetitionID, exp.AccountID, exp.Score, exp.LastUpdateTime))).Should().ThrowAsync<RepositoryException>();
        }

        [Fact]
        public async Task UpdateParticipationTest()
        {
            await ExecSQLFile("participations.sql");
            DateTime crit = DateTime.UtcNow;
            var exp = new PlayerParticipationModel(1, 5, 1000, DateTime.UtcNow);
            await testing.UpdateParticipation(new PlayerParticipation(1, 5, 1000, crit));
            using RepositoriesRealisation.BaseDbContext dbContext = await GetContext();
            dbContext.PlayerParticipation.Find(5, 1).Should().NotBeNull().And.BeEquivalentTo(exp, (a) => a.Excluding(f => f.LastUpdateTime));
        }

        [Fact]
        public async Task UpdateParticipationTestFailure()
        {
            await ExecSQLFile("participations.sql");
            var exp = new PlayerParticipationModel(1, 6, 1000, DateTime.UtcNow);
            await (async () => await testing.UpdateParticipation(new PlayerParticipation(exp.CompetitionID, exp.AccountID, exp.Score, exp.LastUpdateTime))).Should().ThrowAsync<RepositoryException>();
        }

        [Fact]
        public async Task DeleteParticipation()
        {
            await ExecSQLFile("participations.sql");
            await testing.DeleteParticipation(5, 1);
            using RepositoriesRealisation.BaseDbContext dbContext = await GetContext();
            dbContext.PlayerParticipation.Find(5, 1).Should().BeNull();
        }

        [Fact]
        public async Task DeleteParticipationFailure()
        {
            await ExecSQLFile("participations.sql");
            await (async () =>
            await testing.DeleteParticipation(6, 1)).Should().ThrowAsync<RepositoryException>();
        }

        public static object[][] BindCheck = [
            [false, false],
            [true, false],
            [false, true],
            [true, true]
        ];
        [Theory]
        [MemberData(nameof(BindCheck))]
        public async Task GetParticipation(bool bindPlayer, bool bindCompetition)
        {
            await ExecSQLFile("participations.sql");
            PlayerParticipation result = await testing.GetParticipation(2, 1, bindPlayer, bindCompetition);
            using RepositoriesRealisation.BaseDbContext dbContext = await GetContext();
            PlayerParticipationModel md = dbContext.PlayerParticipation.Find(2, 1)!;
            PlayerProfile? etalonP = bindPlayer ? dbContext.PlayerProfiles.Find(2)!.ToCoreModel() : null;
            Competition? etalonC = bindCompetition ? dbContext.Competition.Find(1)!.ToCoreModel() : null;
            result.BindedCompetition.Should().BeEquivalentTo(etalonC);
            result.BindedProfile.Should().BeEquivalentTo(etalonP);
            result.Score.Should().Be(md.Score);
            result.CompetitionId.Should().Be(md.CompetitionID);
            result.PlayerProfileId.Should().Be(md.AccountID);
            result.LastUpdateTime.Should().Be(md.LastUpdateTime);
        }

        [Fact]
        public async Task GetParticipationMissing()
        {
            await ExecSQLFile("participations.sql");
            await (async () => await testing.GetParticipation(2, 2)).Should().ThrowAsync<RepositoryException>();
        }

        public static object[][] DataLimitersAndCheck = [
            [DataLimiter.NoLimit, false, false],
            [DataLimiter.NoLimit, true, false],
            [DataLimiter.NoLimit, false, true],
            [DataLimiter.NoLimit, true, true],
            [new DataLimiter(1, 2), false, false],
            [new DataLimiter(1, 2), true, false],
            [new DataLimiter(1, 2), false, true],
            [new DataLimiter(1, 2), true, true]
        ];
        [Theory]
        [MemberData(nameof(DataLimitersAndCheck))]
        public async Task GetLeaderboard(DataLimiter dt, bool bindPlayer, bool bindCompetition)
        {
            await ExecSQLFile("participations.sql");
            IEnumerable<PlayerParticipation> result = await testing.GetLeaderboard(1, dt, bindPlayer, bindCompetition);
            if (dt.HasNoLimit)
            {
                result.Should().HaveCount(5).And.BeInDescendingOrder(x => x.Score, "thats the leaderboard");
            }
            else
            {
                result.Should().HaveCount(dt.Partition).And.BeInDescendingOrder(x => x.Score, "thats the leaderboard");
            }
        }

        [Theory]
        [MemberData(nameof(DataLimitersAndCheck))]
        public async Task GetPlayerParticipations(DataLimiter dt, bool bindPlayer, bool bindCompetition)
        {
            await ExecSQLFile("participations.sql");
            IEnumerable<PlayerParticipation> result = await testing.GetPlayerParticipations(2, dt, bindPlayer, bindCompetition);
            if (dt.HasNoLimit)
            {
                result.Should().ContainSingle();
            }
            else
            {
                result.Should().BeEmpty();
            }
        }
    }
}
