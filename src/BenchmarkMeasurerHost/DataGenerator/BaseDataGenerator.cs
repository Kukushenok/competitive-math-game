using AutoBogus;
using Bogus;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;

namespace BenchmarkMeasurerHost.DataGenerator
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value)
                ? value
                : value.Length <= maxLength
                    ? value
                    : value[..maxLength];
        }
    }

    internal sealed class BaseDataGenerator : ICompetitionEnvironmentGenerator
    {
        private const string LOCALE = "ru";
        private readonly IAccountRepository accountRepository;
        private readonly IRewardDescriptionRepository rewardKindRepository;
        private readonly ICompetitionRewardRepository competitionRewardRepository;
        private readonly ICompetitionRepository competitionRepository;
        private readonly IPlayerParticipationRepository playerParticipationRepository;
        public BaseDataGenerator(IAccountRepository accountRepository, IRewardDescriptionRepository rewardDescriptionRepository,
            ICompetitionRewardRepository competitionRewardRepository, ICompetitionRepository competitionRepository, IPlayerParticipationRepository playerParticipationRepository)
        {
            this.accountRepository = accountRepository;
            this.competitionRepository = competitionRepository;
            this.competitionRewardRepository = competitionRewardRepository;
            this.playerParticipationRepository = playerParticipationRepository;
            rewardKindRepository = rewardDescriptionRepository;
        }

        private async Task CreateAccounts(EnvironmentSettings currDataSettings)
        {
            Faker<Account> accountFaker = new AutoFaker<Account>(LOCALE)
                .RuleFor(a => a.Login, f => (f.Internet.UserName() + "!" + f.IndexFaker).Truncate(32))
                .RuleFor(a => a.Email, f => f.Internet.Email().Truncate(32));
            var list = accountFaker.Generate(currDataSettings.ParticipantsCount).ToList();

            // foreach(Account acc in list)
            // {
            //    await _accountRepository.CreateAccount(acc, "HASH", new PlayerRole());
            // }
            await Parallel.ForEachAsync(
                list,
                async (x, c) => await accountRepository.CreateAccount(x, "HASH", new PlayerRole()));
        }

        private async Task<List<RewardDescription>> CreateRewardDescriptions(EnvironmentSettings currDataSettings)
        {
            Faker<RewardDescription> rewardFaker = new AutoFaker<RewardDescription>(LOCALE)
                .RuleFor(r => r.Name, f => f.Commerce.ProductName().Truncate(32))
                .RuleFor(r => r.Description, f => f.Lorem.Sentence().Truncate(32));
            await Parallel.ForEachAsync(
                rewardFaker.Generate(currDataSettings.RewardKindCount),
                async (x, c) => await rewardKindRepository.CreateRewardDescription(x));
            return [.. await rewardKindRepository.GetAllRewardDescriptions(DataLimiter.NoLimit)];
        }

        private async Task<Competition> CreateCoreCompetition()
        {
            Faker<Competition> competitionFaker = new AutoFaker<Competition>(LOCALE)
               .RuleFor(cr => cr.Description, f => f.Lorem.Sentence().Truncate(32))
               .RuleFor(cr => cr.Name, f => f.Commerce.ProductName().Truncate(32))
               .RuleFor(cr => cr.StartDate, f => f.Date.Recent(10))
               .RuleFor(cr => cr.EndDate, f => f.Date.Soon(10));

            await competitionRepository.CreateCompetition(competitionFaker.Generate());
            return (await competitionRepository.GetActiveCompetitions()).First();
        }

        private async Task CreateCompetitionRewards(EnvironmentSettings currDataSettings, Competition coreCompetition, List<RewardDescription> rewards)
        {
            // float supposedCount = (float)(currDataSettings.SupposedRewardCount) / (currDataSettings.ParticipantsCount);
            // float batchSuperstition = supposedCount / MathF.Ceiling(supposedCount) / 2;
            // int batchSize = (int)(currDataSettings.ParticipantsCount * batchSuperstition);
            // int batchCount = ((int)MathF.Ceiling(supposedCount)) * 2;
            float batchSuperstition = 0.2f;
            int batchSize = currDataSettings.SupposedRewardCount / 5;
            int batchCount = 5;

            // Генерация CompetitionReward с разными условиями
            Faker<CompetitionReward> competitionRewardFaker = new AutoFaker<CompetitionReward>(LOCALE)
                .RuleFor(cr => cr.RewardDescriptionID, f => f.PickRandom(rewards).Id)
                .RuleFor(cr => cr.CompetitionID, f => coreCompetition.Id)
                .RuleFor(cr => cr.Condition, f =>
                {
                    if (f.Random.Bool())
                    {
                        float offset = (float)f.Random.Double(0, 1.0f - batchSuperstition);
                        return new RankGrantCondition(
                            offset,
                            offset + batchSuperstition);
                    }
                    else
                    {
                        int offset = f.Random.Int(1, currDataSettings.ParticipantsCount - batchSize);
                        return new PlaceGrantCondition(
                            offset,
                            offset + batchSize);
                    }
                });

            await Parallel.ForEachAsync(
                competitionRewardFaker.Generate(batchCount).ToList(),
                async (x, c) => await competitionRewardRepository.CreateCompetitionReward(x));
        }

        private async Task GeneratePlayerParticipations(EnvironmentSettings currDataSettings, Competition coreCompetition)
        {
            Faker<PlayerParticipation> participationFaker = new AutoFaker<PlayerParticipation>()
                .RuleFor(pp => pp.CompetitionId, coreCompetition.Id)
                .RuleFor(pp => pp.PlayerProfileId, f => f.IndexFaker + 1)
                .RuleFor(pp => pp.Score, f => f.Random.Number(0, 1000))
                .RuleFor(pp => pp.LastUpdateTime, f => f.Date.Recent(30));
            await Parallel.ForEachAsync(
                participationFaker.Generate(currDataSettings.ParticipantsCount).ToList(),
                async (x, c) => await playerParticipationRepository.CreateParticipation(x));
        }

        public async Task<Competition> GenerateEnvironment(EnvironmentSettings settings)
        {
            _ = new Random();
            await CreateAccounts(settings);
            List<RewardDescription> rewards = await CreateRewardDescriptions(settings);
            Competition coreCompetition = await CreateCoreCompetition();
            await CreateCompetitionRewards(settings, coreCompetition, rewards);
            await GeneratePlayerParticipations(settings, coreCompetition);
            return coreCompetition;
        }
    }
}
