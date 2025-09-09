using CompetitiveBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using CompetitiveBackend.Core.Objects;
using FluentAssertions;
using RepositoriesRealisation.Models;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace RepositoriesTests.RepositoriesTests
{
    public class PlayerParticipationRepositoryTests : IntegrationTest<IPlayerParticipationRepository>
    {

        public PlayerParticipationRepositoryTests(ITestOutputHelper helper) : base(helper)
        {
        }
        [Fact]
        public async Task CreateParticipationTest()
        {
            await ExecSQLFile("participations.sql");
            PlayerParticipationModel exp = new PlayerParticipationModel(1, 6, 100, DateTime.UtcNow);
            await Testing.CreateParticipation(new PlayerParticipation(exp.CompetitionID, exp.AccountID, exp.Score,exp.LastUpdateTime));
            using var dbContext = await GetContext();
            dbContext.PlayerParticipation.Find(6, 1).Should().NotBeNull().And.BeEquivalentTo(exp, (a) => a.Excluding(f=>f.LastUpdateTime));
        }
        [Fact]
        public async Task CreateParticipationTest_AlreadyExist()
        {
            await ExecSQLFile("participations.sql");
            PlayerParticipationModel exp = new PlayerParticipationModel(1, 5, 100, DateTime.UtcNow);
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.CreateParticipation(new PlayerParticipation(exp.CompetitionID, exp.AccountID, exp.Score, exp.LastUpdateTime)));
            
        }
        [Fact]
        public async Task UpdateParticipationTest()
        {
            await ExecSQLFile("participations.sql");
            DateTime crit = DateTime.UtcNow;
            PlayerParticipationModel exp = new PlayerParticipationModel(1, 5, 1000, DateTime.UtcNow);
            await Testing.UpdateParticipation(new PlayerParticipation(1, 5, 1000, crit));
            using var dbContext = await GetContext();
            dbContext.PlayerParticipation.Find(5, 1).Should().NotBeNull().And.BeEquivalentTo(exp, (a) => a.Excluding(f=>f.LastUpdateTime));
        }
        [Fact]
        public async Task UpdateParticipationTest_Failure()
        {
            await ExecSQLFile("participations.sql");
            PlayerParticipationModel exp = new PlayerParticipationModel(1, 6, 1000, DateTime.UtcNow);
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.UpdateParticipation(new PlayerParticipation(exp.CompetitionID, exp.AccountID, exp.Score, exp.LastUpdateTime)));
        }
        [Fact]
        public async Task DeleteParticipation()
        {
            await ExecSQLFile("participations.sql");
            await Testing.DeleteParticipation(5, 1);
            using var dbContext = await GetContext();
            dbContext.PlayerParticipation.Find(5, 1).Should().BeNull();
        }
        [Fact]
        public async Task DeleteParticipation_Failure()
        {
            await ExecSQLFile("participations.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () =>
            await Testing.DeleteParticipation(6, 1));
        }
        public static object[][] bindCheck = [
            [false, false],
            [true, false],
            [false, true],
            [true, true]
        ];
        [Theory]
        [MemberData(nameof(bindCheck))]
        public async Task GetParticipation(bool bindPlayer, bool bindCompetition)
        {
            await ExecSQLFile("participations.sql");
            var result = await Testing.GetParticipation(2, 1, bindPlayer, bindCompetition);
            using var dbContext = await GetContext();
            PlayerParticipationModel md = dbContext.PlayerParticipation.Find(2, 1)!;
            PlayerProfile? etalonP = bindPlayer? dbContext.PlayerProfiles.Find(2)!.ToCoreModel() : null;
            Competition? etalonC = bindCompetition ? dbContext.Competition.Find(1)!.ToCoreModel() : null;
            result.BindedCompetition.Should().BeEquivalentTo(etalonC);
            result.BindedProfile.Should().BeEquivalentTo(etalonP);
            result.Score.Should().Be(md.Score);
            result.CompetitionId.Should().Be(md.CompetitionID);
            result.PlayerProfileId.Should().Be(md.AccountID);
            result.LastUpdateTime.Should().Be(md.LastUpdateTime);
        }
        [Fact]
        public async Task GetParticipation_Missing()
        {
            await ExecSQLFile("participations.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.GetParticipation(2, 2));
        }
        public static object[][] dataLimitersAndCheck = [
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
        [MemberData(nameof(dataLimitersAndCheck))]
        public async Task GetLeaderboard(DataLimiter dt, bool bindPlayer, bool bindCompetition)
        {
            await ExecSQLFile("participations.sql");
            var result = await Testing.GetLeaderboard(1, dt, bindPlayer, bindCompetition);
            if (dt.HasNoLimit)
                result.Should().HaveCount(5).And.BeInDescendingOrder(x => x.Score, "thats the leaderboard");
            else
                result.Should().HaveCount(dt.Partition).And.BeInDescendingOrder(x => x.Score, "thats the leaderboard");
        }
        [Theory]
        [MemberData(nameof(dataLimitersAndCheck))]
        public async Task GetPlayerParticipations(DataLimiter dt, bool bindPlayer, bool bindCompetition)
        {
            await ExecSQLFile("participations.sql");
            var result = await Testing.GetPlayerParticipations(2, dt, bindPlayer, bindCompetition);
            if (dt.HasNoLimit)
                result.Should().ContainSingle();
            else
                result.Should().BeEmpty();
        }
    }
}
