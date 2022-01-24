using Npgsql;

namespace Lesson_1_2.Interfaces
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

        public ConnectionManager()
        {
            ConnectionString = "Host=localhost;Port=5432;Database=secure_dev;Username=postgres;Password=root;Timeout=180;Command Timeout=180;";
        }

        public NpgsqlConnection GetOpenedConnection()
        {
            Connection = new NpgsqlConnection(ConnectionString);
            Connection.Open();
            return Connection;
        }
    }
}
