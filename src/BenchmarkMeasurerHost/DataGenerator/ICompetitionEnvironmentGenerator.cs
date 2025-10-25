using CompetitiveBackend.Core.Objects;

namespace BenchmarkMeasurerHost.DataGenerator
{
    public interface ICompetitionEnvironmentGenerator
    {
        Task<Competition> GenerateEnvironment(EnvironmentSettings settings);
    }
}
