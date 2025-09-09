using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RewardGranters
{
    public interface IRewardGranter
    {
        public Task GrantRewards(BaseDbContext context, int competitionID);
    }
}
