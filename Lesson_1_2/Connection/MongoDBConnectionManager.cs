using MongoDB.Driver;
using MongoDB.Bson;

namespace Lesson_1_2.Connection
{
    public interface IMongoDBConnectionManager
    {
        public string ConnectionString { get; }
        public IMongoCollection<BsonDocument> Connection { get; set; }
        public IMongoCollection<BsonDocument> GetOpenedConnection(string database, string collection);
    }

    public class MongoDBConnectionManager : IMongoDBConnectionManager
{
        public string ConnectionString { get; }
        public IMongoCollection<BsonDocument> Connection { get; set; }

        public MongoDBConnectionManager(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("MongoDB");
        }

        public IMongoCollection<BsonDocument> GetOpenedConnection(string database, string collection)
        {
            var client = new MongoClient(ConnectionString);
            return client.GetDatabase(database).GetCollection<BsonDocument>(collection);
        }
    }
}
