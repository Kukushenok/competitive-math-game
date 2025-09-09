using BenchmarkMeasurerHost.DataGenerator;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BenchmarkMeasurerHost.TimeMeasurer
{
    class RewardExecuterHost: ITimeMeasurerHost
    {
        private IPlayerRewardRepository repository;
        private ICompetitionEnvironmentGenerator environmentGenerator;
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
            Stopwatch stp = new Stopwatch();
            stp.Start();
            await repository.GrantRewardsFor(1);
            stp.Stop();
            return stp;
        }
    }
}
