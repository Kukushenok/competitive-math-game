using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace BenchmarkMeasurer.Tests
{
    public class DefaultCallsTests : BenchmarkTest
    {
        public DefaultCallsTests(ITestOutputHelper helper) : base(helper)
        {
        }

        protected override string GetName() => "DefaultCalls";
        protected override void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(coll => coll.GrantRewardsWithDefaultCalls());
        }

    }
}
