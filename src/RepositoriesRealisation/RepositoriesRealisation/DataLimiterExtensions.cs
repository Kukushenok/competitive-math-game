using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RepositoriesRealisation
{
    internal static class DataLimiterExtensions
    {
        public static Range ToRange(this DataLimiter limiter)
        {
            if (limiter.HasNoLimit) return Range.All;
            int idx = limiter.PartitionIndex * limiter.Partition;
            return new Range(idx,
                idx + limiter.Partition);
        }
    }
}
