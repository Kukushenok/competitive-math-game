using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BaseUsage.Converters
{
    static class DTOConverter
    {
        public static DataLimiter Convert(this DataLimiterDTO limiter)
        {
            return new DataLimiter(limiter.Page, limiter.Count);
        }
        public static IEnumerable<T> Convert<T, C>(IEnumerable<C> convertable, Func<C, T> converter)
        {
            foreach (C q in convertable) yield return converter(q);
        }
    }
}
