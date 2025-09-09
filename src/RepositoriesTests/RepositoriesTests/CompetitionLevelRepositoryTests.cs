using CompetitiveBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitiveBackend.Core.Objects;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using RepositoriesRealisation.Models;
using CompetitiveBackend.Repositories.Exceptions;
using Docker.DotNet.Models;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample2")]
    public class CompetitionLevelRepositoryTests : IntegrationTest<ICompetitionLevelRepository>
    {
        public CompetitionLevelRepositoryTests(ITestOutputHelper helper) : base(helper)
        {
        }
        [Fact]
        public async Task AddCompetitionLevel()
        {
            await ExecSQLFile("competitions.sql");
            LargeData etalonD = new LargeData([1, 2, 3]);
            LevelDataInfo info = new LevelDataInfo(1, 5, "Android");
            CompetitionLevelDataModel model = new CompetitionLevelDataModel(info);
            CompetitionLevelDataModelData modelData = new CompetitionLevelDataModelData();
            modelData.LevelData = etalonD.Data;
            await Testing.AddCompetitionLevel(etalonD, info);

            await DoDumpings("created_level");
            using var context = await GetContext();
            context.CompetitionLevelModel.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(model, x => x.Excluding(f => f.Id).Excluding(f => f.LevelData));
            context.CompetitionLevelModelData.ToList().Should().ContainSingle().Which.Should().BeEquivalentTo(modelData, x => x.Excluding(f => f.Id).Excluding(f => f.Model));

        }
        [Fact]
        public async Task AddCompetitionLevel_Failure()
        {
            LargeData etalonD = new LargeData([1, 2, 3]);
            LevelDataInfo info = new LevelDataInfo(8, 5, "Android");
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.AddCompetitionLevel(etalonD, info));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task UpdateCompetitionLevelInfo(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            LevelDataInfo info = new LevelDataInfo(1, 10, "MacOS", idx);
            CompetitionLevelDataModel model = new CompetitionLevelDataModel(info);
            await Testing.UpdateCompetitionLevelInfo(info);

            using var context = await GetContext();
            context.CompetitionLevelModel.Find(idx).Should().NotBeNull().And.BeEquivalentTo(model, x => x.Excluding(f => f.Id).Excluding(f => f.LevelData));
        }
        [Fact]
        public async Task UpdateCompetitionLevel_Failure()
        {
            LevelDataInfo info = new LevelDataInfo(8, 5, "Android");
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.UpdateCompetitionLevelInfo(info));
        }
        public static object[][] onetwothree = [[1], [2], [3]];
        [Theory]
        [MemberData(nameof(onetwothree))]
        public async Task UpdateCompetitionLevelData(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            LargeData etalonD = new LargeData([5, 6, 2, 5]);
            CompetitionLevelDataModelData data = new CompetitionLevelDataModelData();
            data.LevelData = etalonD.Data;
            await Testing.UpdateCompetitionLevelData(idx, etalonD);

            using var context = await GetContext();
            context.CompetitionLevelModelData.Find(idx).Should().NotBeNull().And.BeEquivalentTo(data, x => x.Excluding(f => f.Id).Excluding(f => f.Model));
        }
        [Fact]
        public async Task UpdateCompetitionLevelData_Failure()
        {
            LargeData etalonD = new LargeData([5, 6, 2, 5]);
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.UpdateCompetitionLevelData(7, etalonD));
        }

        [Theory]
        [MemberData(nameof(onetwothree))]
        public async Task DeleteCompetitionLevelData(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            await Testing.DeleteCompetitionLevel(idx);

            using var context = await GetContext();
            context.CompetitionLevelModelData.Find(idx).Should().BeNull();
        }
        [Fact]
        public async Task DeleteCompetitionLevelData_Failure()
        {
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.DeleteCompetitionLevel(7));
        }
        public static object[][] entries = [
            [null!, null!, new byte[3] {4, 5, 6}],
            ["Windows", null!, new byte[3] { 7, 8, 9 }],
            ["Android", null!, new byte[3]{4, 5, 6}],
            ["Android", 5, new byte[3]{1, 2, 3}],
            ["Missigno", 10, null!],
            ["Android", 0, null!]
            ];
        [Theory]
        [MemberData(nameof(entries))]
        public async Task GetLevelDataTests(string? platform, int? maxVersion, byte[]? data)
        {
            await ExecSQLFile("competition_levels.sql");
            LargeData dt = await Testing.GetCompetitionLevel(1, platform, maxVersion);
            if(data == null)
            {
                data = Array.Empty<byte>();
            }
            dt.Data.Should().BeEquivalentTo(data);
        }
        [Fact]
        public async Task GetAllLevelData()
        {
            await ExecSQLFile("competition_levels.sql");
            IEnumerable<LevelDataInfo> c = await Testing.GetAllLevelData(1);
            c.ToList().Should().HaveCount(3);
        }
        [Fact]
        public async Task GetAllLevelData_Empty()
        {
            await ExecSQLFile("competitions.sql");
            IEnumerable<LevelDataInfo> c = await Testing.GetAllLevelData(2);
            c.ToList().Should().BeEmpty();
        }
        [Theory]
        [MemberData(nameof(onetwothree))]
        public async Task GetSpecificLevelData(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            LargeData dat = await Testing.GetSpecificCompetitionLevel(idx);
            byte[] garn = { (byte)(idx*3 - 2), (byte)(idx *3 - 1), (byte)(idx *3)};
            dat.Data.Should().NotBeEmpty().And.BeEquivalentTo(garn);
        }
        [Fact]
        public async Task GetSpecificLevelData_Failure()
        {
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.GetSpecificCompetitionLevel(7));
        }
        [Theory]
        [MemberData(nameof(onetwothree))]
        public async Task GetSpecificLevelDataInfo(int idx)
        {
            await ExecSQLFile("competition_levels.sql");
            LevelDataInfo dat = await Testing.GetSpecificCompetitionLevelInfo(idx);
            dat.Id.Should().Be(idx);
            dat.CompetitionID.Should().Be(1);
        }
        [Fact]
        public async Task GetSpecificLevelDataInfo_Failure()
        {
            await ExecSQLFile("competitions.sql");
            await Assert.ThrowsAnyAsync<RepositoryException>(async () => await Testing.GetSpecificCompetitionLevelInfo(7));
        }
    }
}
