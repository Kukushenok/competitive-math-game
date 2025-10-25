using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample2")]
    [Obsolete]
    public class CompetitionLevelRepositoryTests : IntegrationTest<ICompetitionLevelRepository>
    {
        public CompetitionLevelRepositoryTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        [Obsolete]
        public async Task AddCompetitionLevel()
        {
            await ExecSQLFile("competitions.sql");
            var etalonD = new LargeData([1, 2, 3]);
            var info = new LevelDataInfo(1, 5, "Android");
            var model = new CompetitionLevelDataModel(info);
            var modelData = new CompetitionLevelDataModelData
            {
                LevelData = etalonD.Data,
            };
            await testing.AddCompetitionLevel(etalonD, info);

            await DoDumpings("created_level");
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionLevelModel.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(model, x => x.Excluding(f => f.Id).Excluding(f => f.LevelData));
            context.CompetitionLevelModelData.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(modelData, x => x.Excluding(f => f.Id).Excluding(f => f.Model));
        }

        [Fact]
        [Obsolete]
        public async Task AddCompetitionLevelFailure()
        {
            var etalonD = new LargeData([1, 2, 3]);
            var info = new LevelDataInfo(8, 5, "Android");
            await ExecSQLFile("competitions.sql");
            await (async () => await testing.AddCompetitionLevel(etalonD, info)).Should().ThrowAsync<RepositoryException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [Obsolete]
        public async Task UpdateCompetitionLevelInfo(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            var info = new LevelDataInfo(1, 10, "MacOS", idx);
            var model = new CompetitionLevelDataModel(info);
            await testing.UpdateCompetitionLevelInfo(info);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionLevelModel.Find(idx).Should().NotBeNull().And.BeEquivalentTo(model, x => x.Excluding(f => f.Id).Excluding(f => f.LevelData));
        }

        [Fact]
        [Obsolete]
        public async Task UpdateCompetitionLevelFailure()
        {
            var info = new LevelDataInfo(8, 5, "Android");
            await ExecSQLFile("competitions.sql");
            await (async () => await testing.UpdateCompetitionLevelInfo(info)).Should().ThrowAsync<RepositoryException>();
        }

        public static object[][] Onetwothree = [[1], [2], [3]];
        [Theory]
        [MemberData(nameof(Onetwothree))]
        public async Task UpdateCompetitionLevelData(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            var etalonD = new LargeData([5, 6, 2, 5]);
            var data = new CompetitionLevelDataModelData
            {
                LevelData = etalonD.Data,
            };
            await testing.UpdateCompetitionLevelData(idx, etalonD);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionLevelModelData.Find(idx).Should().NotBeNull().And.BeEquivalentTo(data, x => x.Excluding(f => f.Id).Excluding(f => f.Model));
        }

        [Fact]
        public async Task UpdateCompetitionLevelDataFailure()
        {
            var etalonD = new LargeData([5, 6, 2, 5]);
            await ExecSQLFile("competitions.sql");
            await (async () => await testing.UpdateCompetitionLevelData(7, etalonD)).Should().ThrowAsync<RepositoryException>();
        }

        [Theory]
        [MemberData(nameof(Onetwothree))]
        public async Task DeleteCompetitionLevelData(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            await testing.DeleteCompetitionLevel(idx);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionLevelModelData.Find(idx).Should().BeNull();
        }

        [Fact]
        public async Task DeleteCompetitionLevelDataFailure()
        {
            await ExecSQLFile("competitions.sql");
            await (async () => await testing.DeleteCompetitionLevel(7)).Should().ThrowAsync<RepositoryException>();
        }

        public static object[][] Entries = [
            [null!, null!, new byte[3] { 4, 5, 6 }],
            ["Windows", null!, new byte[3] { 7, 8, 9 }],
            ["Android", null!, new byte[3] { 4, 5, 6 }],
            ["Android", 5, new byte[3] { 1, 2, 3 }],
            ["Missigno", 10, null!],
            ["Android", 0, null!]
            ];
        [Theory]
        [MemberData(nameof(Entries))]
        public async Task GetLevelDataTests(string? platform, int? maxVersion, byte[]? data)
        {
            await ExecSQLFile("competition_levels.sql");
            LargeData dt = await testing.GetCompetitionLevel(1, platform, maxVersion);
            data ??= [];

            dt.Data.Should().BeEquivalentTo(data);
        }

        [Fact]
        [Obsolete]
        public async Task GetAllLevelData()
        {
            await ExecSQLFile("competition_levels.sql");
            IEnumerable<LevelDataInfo> c = await testing.GetAllLevelData(1);
            c.ToList().Should().HaveCount(3);
        }

        [Fact]
        [Obsolete]
        public async Task GetAllLevelDataEmpty()
        {
            await ExecSQLFile("competitions.sql");
            IEnumerable<LevelDataInfo> c = await testing.GetAllLevelData(2);
            c.ToList().Should().BeEmpty();
        }

        [Theory]
        [MemberData(nameof(Onetwothree))]
        public async Task GetSpecificLevelData(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            LargeData dat = await testing.GetSpecificCompetitionLevel(idx);
            byte[] garn = [(byte)((idx * 3) - 2), (byte)((idx * 3) - 1), (byte)(idx * 3)];
            dat.Data.Should().NotBeEmpty().And.BeEquivalentTo(garn);
        }

        [Fact]
        public async Task GetSpecificLevelDataFailure()
        {
            await ExecSQLFile("competitions.sql");
            await (async () => await testing.GetSpecificCompetitionLevel(7)).Should().ThrowAsync<RepositoryException>();
        }

        [Theory]
        [MemberData(nameof(Onetwothree))]
        [Obsolete]
        public async Task GetSpecificLevelDataInfo(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            LevelDataInfo dat = await testing.GetSpecificCompetitionLevelInfo(idx);
            dat.Id.Should().Be(idx);
            dat.CompetitionID.Should().Be(1);
        }

        [Fact]
        [Obsolete]
        public async Task GetSpecificLevelDataInfoFailure()
        {
            await ExecSQLFile("competitions.sql");
            await (async () => await testing.GetSpecificCompetitionLevelInfo(7)).Should().ThrowAsync<RepositoryException>();
        }
    }
}
