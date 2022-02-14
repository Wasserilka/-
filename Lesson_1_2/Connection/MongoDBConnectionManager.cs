using MongoDB.Driver;
using Lesson_1_2.DAL.Models;

namespace Lesson_1_2.Connection
{
    public interface IMongoDBConnectionManager
    {
        public string ConnectionString { get; }
        public IMongoCollection<Book> Connection { get; set; }
        public IMongoCollection<Book> GetOpenedConnection(string database, string collection);
    }

    public class MongoDBConnectionManager : IMongoDBConnectionManager
{
        public string ConnectionString { get; }
        public IMongoCollection<Book> Connection { get; set; }

        public MongoDBConnectionManager(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("MongoDB");
        }

        public IMongoCollection<Book> GetOpenedConnection(string database, string collection)
        {
            var client = new MongoClient(ConnectionString);
            return client.GetDatabase(database).GetCollection<Book>(collection);
        }
    }
}
