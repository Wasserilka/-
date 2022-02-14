using Npgsql;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lesson_1_2.Connection
{
    interface IConnectionManager
    {
        public string ConnectionString { get; }
        public NpgsqlConnection Connection { get; set; }
        public NpgsqlConnection GetOpenedConnection();
    }

    class ConnectionManager : IConnectionManager
    {
        public string ConnectionString { get; }
        public NpgsqlConnection Connection { get; set; }

        public ConnectionManager(IConfiguration configuration, string dataBase)
        {
            ConnectionString = configuration.GetConnectionString(dataBase);
        }

        public NpgsqlConnection GetOpenedConnection()
        {
            Connection = new NpgsqlConnection(ConnectionString);
            Connection.Open();
            return Connection;
        }
        public IMongoCollection<BsonDocument> GetOpenedConnection(string database, string collection)
        {
            var client = new MongoClient(ConnectionString);
            return client.GetDatabase(database).GetCollection<BsonDocument>(collection);
        }
    }
}
