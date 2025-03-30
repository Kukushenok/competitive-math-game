using CompetitiveBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using CompetitiveBackend.Core.Objects;
using FluentAssertions;
using CompetitiveBackend.Repositories.Exceptions;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;

namespace RepositoriesTests.RepositoriesTests
{
    public class CompetitionRepositoryUnitTests : IntegrationTest<ICompetitionRepository>
    {
        public CompetitionRepositoryUnitTests(ITestOutputHelper helper) : base(helper)
        {
        }
        [Fact]
        public async Task CreateCompetition_Success()
        {
            Competition c = new Competition("The name", "The description", DateTime.Now, DateTime.Now + TimeSpan.FromSeconds(10));
            await Testing.CreateCompetition(c);
            await DoDumpings("created_competition");
        }

        [Fact]
        public async Task CreateCompetition_UtcSuccess()
        {
            Competition c = new Competition("The name", "The description", DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromSeconds(10));
            await Testing.CreateCompetition(c);
            await DoDumpings("created_competition");
        }
        [Fact]
        public async Task GetCompetitionLevel_Success()
        {
            await ExecSQLFile("competitions.sql");
            LargeData dt = await Testing.GetCompetitionLevel(2);
            dt.Data.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }
        [Fact]
        public async Task GetCompetitionLevel_Empty()
        {
            await ExecSQLFile("competitions.sql");
            LargeData dt = await Testing.GetCompetitionLevel(1);
            dt.Data.Should().BeEmpty();
        }
        [Fact]
        public async Task GetCompetition_Failure()
        {
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.GetCompetition(6));
        }
        [Fact]
        public async Task GetCompetition_Success()
        {
            await ExecSQLFile("competitions.sql");
            Competition etalon = new Competition("ez", "description_ez", new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), 3);
            Competition c = await Testing.GetCompetition(3);
            c.Should().BeEquivalentTo(etalon);
        }
        public static object[][] limiters = [
            [new DataLimiter(0, 3)],
            [new DataLimiter(0, 2)],
            [new DataLimiter(0, 1)],
            [new DataLimiter(1, 1)],
            [new DataLimiter(2, 1)],
            [DataLimiter.NoLimit]
        ];
        [Theory]
        [MemberData(nameof(limiters))]
        public async Task GetAllCompetitions_Empty(DataLimiter dt)
        {
            IEnumerable<Competition> c = await Testing.GetAllCompetitions(dt);
            c.ToList().Should().BeEmpty();
        }
        [Theory]
        [MemberData(nameof(limiters))]
        public async Task GetAllCompetitions_Basic(DataLimiter dt)
        {
            await ExecSQLFile("competitions.sql");
            IEnumerable<Competition> c = await Testing.GetAllCompetitions(dt);
            int cnt = dt.Partition;
            if (dt.HasNoLimit) cnt = 3;
            c.ToList().Should().BeInDescendingOrder(x => x.StartDate).And.HaveCount(cnt);
        }
        private DateTime Offset(DateTime dt, float hours)
        {
            return dt + TimeSpan.FromHours(hours);
        }
        [Fact]
        public async Task GetActualCompetitions_Basic()
        {
            DateTime core = DateTime.UtcNow;
            await Testing.CreateCompetition(new Competition("a", "b", Offset(core, -1), Offset(core, 1)));
            await Testing.CreateCompetition(new Competition("c", "d", Offset(core, -0.5f), Offset(core, 2.0f)));
            await Testing.CreateCompetition(new Competition("e", "f", Offset(core, -0.2f), Offset(core, 0.2f)));
            await Testing.CreateCompetition(new Competition("PAST", "PAST", Offset(core, -0.9f), Offset(core, -0.2f)));
            await Testing.CreateCompetition(new Competition("FUTURE", "FUTURE", Offset(core, 0.2f), Offset(core, 0.9f)));
            IEnumerable<Competition> c = await Testing.GetActiveCompetitions();
            c.ToList().Should().BeInDescendingOrder(x => x.StartDate).And.HaveCount(3);
        }
        [Fact]
        public async Task UpdateCompetition_Failure()
        {
            DateTime core = DateTime.UtcNow;
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.UpdateCompetition(
                new Competition("a", "b", Offset(core, -1), Offset(core, 1), 10)));
        }
        [Fact]
        public async Task UpdateCompetition_Failure2()
        {
            DateTime core = DateTime.UtcNow;
            await Assert.ThrowsAsync<IncorrectOperationException>(async () => await Testing.UpdateCompetition(
                new Competition("a", "b", Offset(core, -1), Offset(core, 1))));
        }
        [Fact]
        public async Task UpdateCompetition_Success()
        {
            await ExecSQLFile("competitions.sql");
            DateTime core = DateTime.UtcNow;

            Competition etalon = new Competition("a", "b", Offset(core, -1), Offset(core, 1), 1);
            CompetitionModel sample = new CompetitionModel(etalon.Id!.Value, etalon.Name, etalon.Description, etalon.StartDate, etalon.EndDate);
            await Testing.UpdateCompetition(etalon);

            using BaseDbContext context = await GetContext();
            context.Competition.Find(1).Should().BeEquivalentTo(sample, options => options.Excluding(x => x.StartTime).Excluding(x => x.EndTime));
            await DoDumpings("updated_first");
        }
        [Fact]
        public async Task SetCompetitionLevel_Failure()
        {
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.SetCompetitionLevel(6, new([1, 7, 2])));
        }
        [Fact]
        public async Task SetCompetitionLevel_Success()
        {
            await ExecSQLFile("competitions.sql");
            LargeData dt = new LargeData([5, 6, 7]);
            await Testing.SetCompetitionLevel(1, dt);
            using BaseDbContext context = await GetContext();
            context.Competition.Find(1)!.LevelData.Should().BeEquivalentTo(dt.Data);
        }

    }
}
