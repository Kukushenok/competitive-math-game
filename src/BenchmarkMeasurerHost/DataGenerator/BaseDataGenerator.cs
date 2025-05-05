using AutoBogus;
using Bogus;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class BaseDataGenerator: ICompetitionEnvironmentGenerator
    {
        private const string LOCALE = "ru";
        private IAccountRepository _accountRepository;
        private IRewardDescriptionRepository _rewardKindRepository;
        private ICompetitionRewardRepository _competitionRewardRepository;
        private ICompetitionRepository _competitionRepository;
        private IPlayerParticipationRepository _playerParticipationRepository;
        public BaseDataGenerator(IAccountRepository  accountRepository, IRewardDescriptionRepository rewardDescriptionRepository,
            ICompetitionRewardRepository competitionRewardRepository, ICompetitionRepository competitionRepository, IPlayerParticipationRepository playerParticipationRepository)
        {
            _accountRepository = accountRepository;
            _competitionRepository = competitionRepository;
            _competitionRewardRepository = competitionRewardRepository;
            _playerParticipationRepository = playerParticipationRepository;
            _rewardKindRepository = rewardDescriptionRepository;
        }
        private async Task CreateAccounts(EnvironmentSettings currDataSettings)
        {
            var accountFaker = new AutoFaker<Account>(LOCALE)
                .RuleFor(a => a.Login, f => (f.Internet.UserName() + "!" + f.IndexFaker).Truncate(32))
                .RuleFor(a => a.Email, f => f.Internet.Email().Truncate(32));
            var list = accountFaker.Generate(currDataSettings.ParticipantsCount).ToList();
            //foreach(Account acc in list)
            //{
            //    await _accountRepository.CreateAccount(acc, "HASH", new PlayerRole());
            //}
            await Parallel.ForEachAsync(list,
                async (x, c) => await _accountRepository.CreateAccount(x, "HASH", new PlayerRole()));
        }
        private async Task<List<RewardDescription>> CreateRewardDescriptions(EnvironmentSettings currDataSettings)
        {
            var rewardFaker = new AutoFaker<RewardDescription>(LOCALE)
                .RuleFor(r => r.Name, f => f.Commerce.ProductName().Truncate(32))
                .RuleFor(r => r.Description, f => f.Lorem.Sentence().Truncate(32));
            await Parallel.ForEachAsync(rewardFaker.Generate(currDataSettings.RewardKindCount),
                async (x, c) => await _rewardKindRepository.CreateRewardDescription(x));
            return (await _rewardKindRepository.GetAllRewardDescriptions(DataLimiter.NoLimit)).ToList();
        }
        private async Task<Competition> CreateCoreCompetition()
        {
            var competitionFaker = new AutoFaker<Competition>(LOCALE)
               .RuleFor(cr => cr.Description, f => f.Lorem.Sentence().Truncate(32))
               .RuleFor(cr => cr.Name, f => f.Commerce.ProductName().Truncate(32))
               .RuleFor(cr => cr.StartDate, f => f.Date.Recent(10))
               .RuleFor(cr => cr.EndDate, f => f.Date.Soon(10));

            await _competitionRepository.CreateCompetition(competitionFaker.Generate());
            return (await _competitionRepository.GetActiveCompetitions()).First();
        }
        private async Task CreateCompetitionRewards(EnvironmentSettings currDataSettings, Competition coreCompetition, List<RewardDescription> rewards)
        {
            //float supposedCount = (float)(currDataSettings.SupposedRewardCount) / (currDataSettings.ParticipantsCount);
            //float batchSuperstition = supposedCount / MathF.Ceiling(supposedCount) / 2;
            //int batchSize = (int)(currDataSettings.ParticipantsCount * batchSuperstition);
            //int batchCount = ((int)MathF.Ceiling(supposedCount)) * 2;
            float batchSuperstition = 0.2f;
            int batchSize = currDataSettings.SupposedRewardCount / 5;
            int batchCount = 5;
            // Генерация CompetitionReward с разными условиями
            var competitionRewardFaker = new AutoFaker<CompetitionReward>(LOCALE)
                .RuleFor(cr => cr.RewardDescriptionID, f => f.PickRandom(rewards).Id)
                .RuleFor(cr => cr.CompetitionID, f => coreCompetition.Id)
                .RuleFor(cr => cr.Condition, f => {
                    if (f.Random.Bool())
                    {
                        float offset = (float)f.Random.Double(0, 1.0f - batchSuperstition);
                        return new RankGrantCondition(
                            offset,
                            offset + batchSuperstition
                        );
                    }
                    else
                    {
                        int offset = f.Random.Int(1, currDataSettings.ParticipantsCount - batchSize);
                        return new PlaceGrantCondition(
                            offset,
                            offset + batchSize
                        );
                    }
                });

            await Parallel.ForEachAsync(competitionRewardFaker.Generate(batchCount).ToList(),
                async (x, c) => await _competitionRewardRepository.CreateCompetitionReward(x));
        }
        private async Task GeneratePlayerParticipations(EnvironmentSettings currDataSettings, Competition coreCompetition)
        {
            var participationFaker = new AutoFaker<PlayerParticipation>()
                .RuleFor(pp => pp.CompetitionId, coreCompetition.Id)
                .RuleFor(pp => pp.PlayerProfileId, f => f.IndexFaker + 1)
                .RuleFor(pp => pp.Score, f => f.Random.Number(0, 1000))
                .RuleFor(pp => pp.LastUpdateTime, f => f.Date.Recent(30));
            await Parallel.ForEachAsync(participationFaker.Generate(currDataSettings.ParticipantsCount).ToList(), 
                async (x, c) => await _playerParticipationRepository.CreateParticipation(x));
        }
        public async Task<Competition> GenerateEnvironment(EnvironmentSettings settings)
        {
            var random = new Random();
            await CreateAccounts(settings);
            var rewards = await CreateRewardDescriptions(settings);
            var coreCompetition = await CreateCoreCompetition();
            await CreateCompetitionRewards(settings,coreCompetition, rewards);
            await GeneratePlayerParticipations(settings,coreCompetition);
            return coreCompetition;
        }
    }
}
