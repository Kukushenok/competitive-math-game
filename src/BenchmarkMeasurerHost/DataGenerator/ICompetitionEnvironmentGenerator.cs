using CompetitiveBackend.Core.Objects;

namespace BenchmarkMeasurerHost.DataGenerator
{
    public interface ICompetitionEnvironmentGenerator
    {
        public Task<Competition> GenerateEnvironment(EnvironmentSettings settings);
    }
}
