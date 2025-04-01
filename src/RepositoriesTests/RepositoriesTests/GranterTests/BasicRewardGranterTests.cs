using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests.GranterTests
{
    public class BasicRewardGranterTests : BaseGranterTests
    {
        public BasicRewardGranterTests(ITestOutputHelper helper) : base(helper)
        {
        }

        protected override string Name => "basic";

        protected override void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddRepositories(options => options.GrantRewardsWithDefaultCalls());
        }
    }
}
