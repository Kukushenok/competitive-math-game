using Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDBRepositoryRealisation.RepositoriesImplementation.MongoConnectionSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBRepositoryRealisation.RepositoriesImplementation
{
    public interface IAutoIncrementManager
    {
        public Task<int> GetID<T>(IMongoConnection conn);
    }
    class AutoIncrementEntity
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId ID;
        [BsonElement("entityName")]
        public string EntityName;
        [BsonElement("incremented")]
        public int newID;
        public AutoIncrementEntity(string entityName, int id = 0)
        {
            EntityName = entityName;
            newID = id;
        }
        public string ToJson()
        {
            return this.ToJson<AutoIncrementEntity>();
        }
    }
    class AutoIncrementManager : IAutoIncrementManager
    {
        public async Task<int> GetID<T>(IMongoConnection conn)
        {
            string entity = typeof(T).Name;
            var filter = Builders<AutoIncrementEntity>.Filter.Eq(x => x.EntityName, entity);
            var collection = conn.Database.GetCollection<AutoIncrementEntity>("autoincr");
            var ent = (await collection.FindAsync(filter)).SingleOrDefault();
            if(ent == null)
            {
                ent = new AutoIncrementEntity(entity);
                await collection.InsertOneAsync(ent);
            }
            await collection.UpdateOneAsync(filter,
                Builders<AutoIncrementEntity>.Update.Inc(x=>x.newID, 1));
            return ent.newID;

        }
    }
}
