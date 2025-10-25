using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using Xunit.Abstractions;

namespace BenchmarkMeasurer.Tests
{
    [Collection("Sequential")]
    public class StoredProcedureTests : BenchmarkDumpReaderTest
    {
        public StoredProcedureTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        protected override string GetName()
        {
            return "StoredProcedure";
        }

        protected override void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(coll => coll.GrantRewardsWithStoredProcedure());
        }
    }
}
