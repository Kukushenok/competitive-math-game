using AwesomeAssertions;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using RepositoriesRealisation;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample2")]
    public class CompetitionRepositoryUnitTests : IntegrationTest<ICompetitionRepository>
    {
        public CompetitionRepositoryUnitTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task CreateCompetitionSuccess()
        {
            var c = new Competition("The name", "The description", DateTime.Now, DateTime.Now + TimeSpan.FromSeconds(10));
            await testing.CreateCompetition(c);
            await DoDumpings("created_competition");
        }

        [Fact]
        public async Task CreateCompetitionUtcSuccess()
        {
            var c = new Competition("The name", "The description", DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromSeconds(10));
            await testing.CreateCompetition(c);
            await DoDumpings("created_competition");
        }

        // [Fact]
        // public async Task GetCompetitionLevel_Success()
        // {
        //    await ExecSQLFile("competitions.sql");
        //    LargeData dt = await Testing.GetCompetitionLevel(2);
        //    dt.Data.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        // }
        // [Fact]
        // public async Task GetCompetitionLevel_Empty()
        // {
        //    await ExecSQLFile("competitions.sql");
        //    LargeData dt = await Testing.GetCompetitionLevel(1);
        //    dt.Data.Should().BeEmpty();
        // }
        [Fact]
        public async Task GetCompetitionFailure()
        {
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<MissingDataException>(async () => await testing.GetCompetition(6));
        }

        [Fact]
        public async Task GetCompetitionSuccess()
        {
            await ExecSQLFile("competitions.sql");
            var etalon = new Competition("ez", "description_ez", new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), 3);
            Competition c = await testing.GetCompetition(3);
            c.Should().BeEquivalentTo(etalon);
        }

        public static readonly object[][] Limiters = [
            [new DataLimiter(0, 3)],
            [new DataLimiter(0, 2)],
            [new DataLimiter(0, 1)],
            [new DataLimiter(1, 1)],
            [new DataLimiter(2, 1)],
            [DataLimiter.NoLimit]
        ];
        [Theory]
        [MemberData(nameof(Limiters))]
        public async Task GetAllCompetitionsEmpty(DataLimiter dt)
        {
            IEnumerable<Competition> c = await testing.GetAllCompetitions(dt);
            c.ToList().Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(Limiters))]
        public async Task GetAllCompetitionsBasic(DataLimiter dt)
        {
            await ExecSQLFile("competitions.sql");
            IEnumerable<Competition> c = await testing.GetAllCompetitions(dt);
            int cnt = dt.Partition;
            if (dt.HasNoLimit)
            {
                cnt = 3;
            }

            c.ToList().Should().BeInDescendingOrder(x => x.StartDate).And.HaveCount(cnt);
        }

        private static DateTime Offset(DateTime dt, float hours)
        {
            return dt + TimeSpan.FromHours(hours);
        }

        [Fact]
        public async Task GetActualCompetitionsBasic()
        {
            DateTime core = DateTime.UtcNow;
            await testing.CreateCompetition(new Competition("a", "b", Offset(core, -1), Offset(core, 1)));
            await testing.CreateCompetition(new Competition("c", "d", Offset(core, -0.5f), Offset(core, 2.0f)));
            await testing.CreateCompetition(new Competition("e", "f", Offset(core, -0.2f), Offset(core, 0.2f)));
            await testing.CreateCompetition(new Competition("PAST", "PAST", Offset(core, -0.9f), Offset(core, -0.2f)));
            await testing.CreateCompetition(new Competition("FUTURE", "FUTURE", Offset(core, 0.2f), Offset(core, 0.9f)));
            IEnumerable<Competition> c = await testing.GetActiveCompetitions();
            c.ToList().Should().BeInDescendingOrder(x => x.StartDate).And.HaveCount(3);
        }

        [Fact]
        public async Task UpdateCompetitionFailure()
        {
            DateTime core = DateTime.UtcNow;
            await Assert.ThrowsAnyAsync<MissingDataException>(async () => await testing.UpdateCompetition(
                new Competition("a", "b", Offset(core, -1), Offset(core, 1), 10)));
        }

        [Fact]
        public async Task UpdateCompetitionFailure2()
        {
            DateTime core = DateTime.UtcNow;
            await Assert.ThrowsAsync<IncorrectOperationException>(async () => await testing.UpdateCompetition(
                new Competition("a", "b", Offset(core, -1), Offset(core, 1))));
        }

        [Fact]
        public async Task UpdateCompetitionSuccess()
        {
            await ExecSQLFile("competitions.sql");
            DateTime core = DateTime.UtcNow;

            var etalon = new Competition("a", "b", Offset(core, -1), Offset(core, 1), 1);
            var sample = new CompetitionModel(etalon.Id!.Value, etalon.Name, etalon.Description, etalon.StartDate, etalon.EndDate);
            await testing.UpdateCompetition(etalon);

            using BaseDbContext context = await GetContext();
            context.Competition.Find(1).Should().BeEquivalentTo(sample, options => options.Excluding(x => x.StartTime).Excluding(x => x.EndTime));
            await DoDumpings("updated_first");
        }
    }
}
