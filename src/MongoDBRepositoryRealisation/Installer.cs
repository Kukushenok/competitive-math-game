using CompetitiveBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDBRepositoryRealisation.RepositoriesImplementation;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using Moq;
using Repositories.Objects;
using System.ComponentModel;

namespace MongoDBRepositoryRealisation
{
    public static class Installer
    {
        public static IServiceCollection UseMongoDBRepositories(this IServiceCollection coll)
        {
            coll.AddScoped<IAccountRepository, AccountRepository>();
            coll.AddScoped<ISessionRepository, SessionRepository>();
            coll.AddScoped<IMongoConnectionCreator, MongoDatabaseConnCreator>();
            coll.AddScoped<SessionRepositoryConfiguration>();
            coll.AddScoped<IAutoIncrementManager, AutoIncrementManager>();
            coll.AddScoped<ICompetitionRepository, CompetitionRepository>();
            coll.AddScoped<IPlayerProfileRepository, PlayerProfileRepository>();
            coll.AddScoped<IRewardDescriptionRepository, RewardDescriptionRepository>();
            coll.AddScoped<IPlayerRewardRepository, PlayerRewardRepository>();
            coll.AddScoped<IPlayerParticipationRepository, PlayerParticipationRepository>();
            coll.AddScoped<ICompetitionRewardRepository, CompetitionRewardRepository>();
            coll.Dull<IRepositoryPrivilegySetting>();
            return coll;
        }
        private static IServiceCollection Dull<T>(this IServiceCollection coll) where T: class
        {
            coll.AddScoped(_ => new Mock<T>().Object);
            return coll;
        }
    }
}
