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
            Stopwatch stp = new Stopwatch();
            Competition c =await environmentGenerator.GenerateEnvironment(settings);
            stp.Start();
            await repository.GrantRewardsFor(c.Id!.Value);
            stp.Stop();
            return stp;
        }
    }
}
