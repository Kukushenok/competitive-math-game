using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class DTOConverter
    {
        public static DataLimiter Convert(this DataLimiterDTO limiter)
        {
            return new DataLimiter(limiter.Page, limiter.Count);
        }

        public static IEnumerable<T> Convert<T, TOther>(IEnumerable<TOther> convertable, Func<TOther, T> converter)
        {
            foreach (TOther q in convertable)
            {
                yield return converter(q);
            }
        }
    }
}
