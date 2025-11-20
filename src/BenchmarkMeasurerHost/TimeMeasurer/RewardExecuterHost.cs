using System.Diagnostics;
using BenchmarkMeasurerHost.DataGenerator;
using CompetitiveBackend.Repositories;

namespace BenchmarkMeasurerHost.TimeMeasurer
{
    internal sealed class RewardExecuterHost : ITimeMeasurerHost
    {
        private readonly IPlayerRewardRepository repository;
        private readonly ICompetitionEnvironmentGenerator environmentGenerator;
        public RewardExecuterHost(IPlayerRewardRepository repository, ICompetitionEnvironmentGenerator environmentGenerator)
        {
            this.repository = repository;
            this.environmentGenerator = environmentGenerator;
        }

        public async Task<Stopwatch> Measure(EnvironmentSettings settings)
        {
            await environmentGenerator.GenerateEnvironment(settings);
            return await Measure();
        }

        public async Task<Stopwatch> Measure()
        {
            var stp = new Stopwatch();
            stp.Start();
            await repository.GrantRewardsFor(1);
            stp.Stop();
            return stp;
        }
    }
}
