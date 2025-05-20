using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup
{
    public class MongoDatabaseConnCreator: IMongoConnectionCreator
    {
        private IConfiguration conf;
        public MongoDatabaseConnCreator(IConfiguration conf)
        {
            this.conf = conf;
        }
        public IMongoConnection Create()
        {
            var client = new MongoClient(conf.GetConnectionString("mongodb") ?? "mongodb://root:mongo_password@mongo:27017/");
            var mongoDatabase = client.GetDatabase("maindb");
            return new MongoConnection(mongoDatabase, client);
        }
    }
}
