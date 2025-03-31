using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RewardCondition
{
    public class RankGrantCondition : GrantCondition
    {
        public override string Type => "rank";
        public float minRank;
        public float maxRank;
        public RankGrantCondition(float minRank, float maxRank)
        {
            this.minRank = minRank;
            this.maxRank = maxRank;
        }
    }
}
