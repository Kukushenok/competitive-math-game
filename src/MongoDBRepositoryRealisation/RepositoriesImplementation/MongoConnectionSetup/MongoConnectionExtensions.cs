using Model;
using MongoDB.Driver;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup
{
    public static class MongoConnectionExtensions
    {
        public const string REWARD_DESCRIPTION_SCHEMA = "rewardDescription";
        public const string ACCOUNT_SCHEMA = "account";
        public const string COMPETITION_SCHEMA = "competition";
        public const string PLAYER_REWARD_SCHEMA = "playerReward";
        public const string PLAYER_PARTICIPATION_SCHEMA = "playerParticipation";
        public const string COMPETITION_REWARD_SCHEMA = "competitionReward";
        public static IMongoCollection<RewardDescriptionEntity> RewardDescriptions(this IMongoConnection conn) 
            => conn.Database.GetCollection<RewardDescriptionEntity>(REWARD_DESCRIPTION_SCHEMA);
        public static IMongoCollection<AccountEntity> Account(this IMongoConnection conn)
            => conn.Database.GetCollection<AccountEntity>(ACCOUNT_SCHEMA);

        public static IMongoCollection<CompetitionEntity> Competition(this IMongoConnection conn) =>
            conn.Database.GetCollection<CompetitionEntity>(COMPETITION_SCHEMA);

        public static IMongoCollection<PlayerRewardEntity> PlayerReward(this IMongoConnection conn) =>
                conn.Database.GetCollection<PlayerRewardEntity>(PLAYER_REWARD_SCHEMA);

        public static IMongoCollection<PlayerParticipationEntity> PlayerParticipation(this IMongoConnection conn) =>
            conn.Database.GetCollection<PlayerParticipationEntity>(PLAYER_PARTICIPATION_SCHEMA);

        public static IMongoCollection<CompetitionRewardEntity> CompetitionReward(this IMongoConnection conn) =>
            conn.Database.GetCollection<CompetitionRewardEntity>(COMPETITION_REWARD_SCHEMA);
    }
}
