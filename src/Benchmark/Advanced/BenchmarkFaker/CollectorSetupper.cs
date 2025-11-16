using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using Repositories.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Services.ExtraTools;
using static System.Formats.Asn1.AsnWriter;

namespace PrometheusCollectorSetupper
{
    public static class BenchmarkFakerSetupper
    {
        public static async Task<int> BenchmarkBypass(IServiceProvider prv, int minutes)
        {
            using IServiceScope scope = prv.CreateScope();

            IRepositoryPrivilegySetting privilegies = scope.ServiceProvider.GetRequiredService<IRepositoryPrivilegySetting>();
            privilegies.SetPrivilegies("admin");
            int compID = await GetCompID(scope, minutes);
            await InitRewards(scope, compID);
            return compID;
        }

        private static async Task<int> GetCompID(IServiceScope scope, int minutes)
        {
            ICompetitionRepository compRepo = scope.ServiceProvider.GetRequiredService<ICompetitionRepository>();
            IRiddleRepository riddleRepo = scope.ServiceProvider.GetRequiredService<IRiddleRepository>();
            IRiddleSettingsRepository riddleSettingsRepo = scope.ServiceProvider.GetRequiredService<IRiddleSettingsRepository>();
            int result = (await InitializeCompetition(compRepo, riddleSettingsRepo, riddleRepo, minutes)).Id!.Value;
            ICompetitionRewardScheduler scheduler = scope.ServiceProvider.GetRequiredService<ICompetitionRewardScheduler>();
            await scheduler.OnCompetitionCreated(await compRepo.GetCompetition(result));
            return result;
        }

        private static async Task InitRewards(IServiceScope scope, int compID)
        {
            ICompetitionRewardRepository compReward = scope.ServiceProvider.GetRequiredService<ICompetitionRewardRepository>();
            IRewardDescriptionRepository rewardDescr = scope.ServiceProvider.GetRequiredService<IRewardDescriptionRepository>();
            await InitializeRewards(compReward, rewardDescr, compID);
        }

        private static IEnumerable<RiddleAnswer> YieldFakeRandom(Random rnd, int A, int B)
        {
            int count = rnd.Next(0, 5);
            while (count > 0)
            {
                yield return new RiddleAnswer($"{A + B + (rnd.Next(1, 6) * (1 - (2 * rnd.Next(0, 2))))}");
                count--;
            }
        }

        private static GrantCondition GetGrantCondition(Random rnd)
        {
            if(rnd.Next() % 2 == 0)
            {
                int place = 1 + (rnd.Next() % 200);
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
            RewardDescription? rewardKind = (await rwd.GetAllRewardDescriptions(new DataLimiter(0, 1))).FirstOrDefault();
            if (rewardKind == null)
            {
                await rwd.CreateRewardDescription(new RewardDescription("A", "B"));
                rewardKind = (await rwd.GetAllRewardDescriptions(new DataLimiter(0, 1))).First();
            }

            for (int i = 0; i < 20; i++)
            {
                await rewardRepository.CreateCompetitionReward(new CompetitionReward(
                    rewardKind.Id!.Value,
                    compID,
                    string.Empty,
                    string.Empty,
                    GetGrantCondition(rnd)));
            }
        }

        private static async Task CreateRiddle(IRiddleRepository riddleRepo, Random rnd, int compID)
        {
            int a = rnd.Next(10, 100);
            int b = rnd.Next(10, 100);
            var info = new RiddleInfo(
                compID,
                $"{a} + {b} = ?",
                [.. YieldFakeRandom(rnd, a, b)],
                new RiddleAnswer($"{a + b}"));
            await riddleRepo.CreateRiddle(info);
        }

        private static async Task<Competition> InitializeCompetition(ICompetitionRepository P, IRiddleSettingsRepository settingsRepo, IRiddleRepository riddleRepo, int minutes)
        {
            var rnd = new Random();
            string randname = $"R{DateTime.UtcNow.Ticks}{rnd.Next(0, 10000)}";
            await P.CreateCompetition(new Competition(randname, "haii! :3", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMinutes(minutes)));
            Competition[] createdComps = [.. await P.GetActiveCompetitions()];
            Competition result = createdComps.First(x => x.Name == randname);
            for (int i = 0; i < 30; i++)
            {
                await CreateRiddle(riddleRepo, rnd, result.Id!.Value);
            }

            await settingsRepo.UpdateRiddleSettings(result.Id!.Value, new RiddleGameSettings(
                10,
                -10,
                10,
                null,
                0));
            return result;
        }
    }
}
