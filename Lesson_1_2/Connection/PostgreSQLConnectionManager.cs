using Npgsql;

namespace Lesson_1_2.Connection
{
    public interface IPostgreSQLConnectionManager
    {
        public string ConnectionString { get; }
        public NpgsqlConnection Connection { get; set; }
        public NpgsqlConnection GetOpenedConnection();
    }

    public class PostgreSQLConnectionManager : IPostgreSQLConnectionManager
    {
        public string ConnectionString { get; }
        public NpgsqlConnection Connection { get; set; }

        public PostgreSQLConnectionManager(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("PostgreSQL");
        }

        public NpgsqlConnection GetOpenedConnection()
        {
            Connection = new NpgsqlConnection(ConnectionString);
            Connection.Open();
            return Connection;
        }
    }
}
