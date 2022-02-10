using Npgsql;

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

        public ConnectionManager(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public NpgsqlConnection GetOpenedConnection()
        {
            Connection = new NpgsqlConnection(ConnectionString);
            Connection.Open();
            return Connection;
        }
    }
}
