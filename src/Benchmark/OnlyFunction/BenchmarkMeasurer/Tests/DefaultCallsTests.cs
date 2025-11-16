using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using Xunit.Abstractions;

namespace BenchmarkMeasurer.Tests
{
    [Collection("Sequential")]
    public class DefaultCallsTests : BenchmarkDumpReaderTest
    {
        public DefaultCallsTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        protected override string GetName()
        {
            return "DefaultCalls";
        }

        protected override void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(coll => coll.GrantRewardsWithDefaultCalls());
        }
    }
}
