using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using Repositories.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Services.ExtraTools;

namespace PrometheusCollectorSetupper
{
    public static class BenchmarkFakerSetupper
    {
        public static async Task<int> BenchmarkBypass(IServiceProvider prv, int minutes)
        {
            using (var scope = prv.CreateScope())
            {
                var privilegies = scope.ServiceProvider.GetRequiredService<IRepositoryPrivilegySetting>();
                privilegies.SetPrivilegies("admin");
                var compRepo = scope.ServiceProvider.GetRequiredService<ICompetitionRepository>();
                var riddleRepo = scope.ServiceProvider.GetRequiredService<IRiddleRepository>();
                var riddleSettingsRepo = scope.ServiceProvider.GetRequiredService<IRiddleSettingsRepository>();
                int compID = (await InitializeCompetition(compRepo, riddleSettingsRepo, riddleRepo, minutes)).Id!.Value;
                var compReward = scope.ServiceProvider.GetRequiredService<ICompetitionRewardRepository>();
                var rewardDescr = scope.ServiceProvider.GetRequiredService<IRewardDescriptionRepository>();
                await InitializeRewards(compReward, rewardDescr, compID);
                var scheduler = scope.ServiceProvider.GetRequiredService<ICompetitionRewardScheduler>();
                await scheduler.OnCompetitionCreated(await compRepo.GetCompetition(compID));
                return compID;
            }
        }
        private static IEnumerable<RiddleAnswer> YieldFakeRandom(Random rnd, int A, int B)
        {
            var count = rnd.Next(0, 5);
            while(count > 0)
            {
                yield return new RiddleAnswer($"{A + B + rnd.Next(1, 6) * (1 - 2 * rnd.Next(0, 2))}");
                count--; 
            }
        }
        private static GrantCondition GetGrantCondition(Random rnd)
        {
            if(rnd.Next() % 2 == 0)
            {
                int place = 1 + rnd.Next() % 200;
                return new PlaceGrantCondition(place, place + 50);
            } 
            else
            {
                float ratio1 = (float)rnd.NextDouble() * 0.75f;
                return new RankGrantCondition(ratio1, ratio1 + 0.2f);
            }
        }
        private static async Task InitializeRewards(ICompetitionRewardRepository rewardRepository, IRewardDescriptionRepository rwd, int compID)
        {
            var rnd = new Random();
            var rewardKind = (await rwd.GetAllRewardDescriptions(new DataLimiter(0, 1))).FirstOrDefault();
            if(rewardKind == null)
            {
                await rwd.CreateRewardDescription(new RewardDescription("A", "B"));
                rewardKind = (await rwd.GetAllRewardDescriptions(new DataLimiter(0, 1))).First();
            }
            for(int i = 0; i < 20; i++)
            {
                await rewardRepository.CreateCompetitionReward(new CompetitionReward(rewardKind.Id!.Value, compID, "", "",
                    GetGrantCondition(rnd)));
            }
        }
        private static async Task<Competition> InitializeCompetition(ICompetitionRepository P, IRiddleSettingsRepository settingsRepo, IRiddleRepository riddleRepo, int minutes)
        {
            var rnd = new Random();
            string randname = "R" + DateTime.UtcNow.Ticks.ToString() + rnd.Next(0, 10000).ToString();
            await P.CreateCompetition(new Competition(randname, "haii! :3", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMinutes(minutes)));
            var createdComps = (await P.GetActiveCompetitions()).ToArray();
            var result = createdComps.Where(x=>x.Name== randname).First();
            for(int i = 0; i < 30; i++)
            {
                int A = rnd.Next(10, 100);
                int B = rnd.Next(10, 100);
                RiddleInfo info = new RiddleInfo(
                    result.Id!.Value,
                    $"{A} + {B} = ?",
                    YieldFakeRandom(rnd, A, B).ToList(),
                    new RiddleAnswer($"{A + B}")
                    );
                await riddleRepo.CreateRiddle(info);
            }
            await settingsRepo.UpdateRiddleSettings(result.Id!.Value, new RiddleGameSettings(
                10,
                -10,
                10,
                null,
                0
                ));
            return result;
        }
    }
}
