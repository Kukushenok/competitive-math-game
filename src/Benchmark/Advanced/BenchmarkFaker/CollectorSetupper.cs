using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Core;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using Repositories.Objects;

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
                var lx = (await compRepo.GetActiveCompetitions()).ToArray();
                if (lx.Length <= 0)
                {

                    return (await InitializeCompetition(compRepo, riddleSettingsRepo, riddleRepo, minutes)).Id!.Value;
                }
                return lx[0].Id!.Value;
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
        private static async Task<Competition> InitializeCompetition(ICompetitionRepository P, IRiddleSettingsRepository settingsRepo, IRiddleRepository riddleRepo, int minutes)
        {
            var rnd = new Random();
            await P.CreateCompetition(new Competition("BOOTSTRAPPED", "HELLO", DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMinutes(minutes)));
            var createdComps = (await P.GetActiveCompetitions()).ToArray();
            var result = createdComps[0];
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
