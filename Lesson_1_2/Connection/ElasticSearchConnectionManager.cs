using Nest;

namespace Lesson_1_2.Connection
{
    public interface IElasticSearchConnectionManager
    {
        public string ConnectionString { get; }
        public string DefaultIndex { get; }
        public IElasticClient Connection { get; set; }
        public IElasticClient GetOpenedConnection();
    }

    public class ElasticSearchConnectionManager : IElasticSearchConnectionManager
    {
        public string ConnectionString { get; }
        public string DefaultIndex { get; }
        public IElasticClient Connection { get; set; }

        public ElasticSearchConnectionManager(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("ElasticSearch");
            DefaultIndex = "books";
        }

        public IElasticClient GetOpenedConnection()
        {
            var settings = new ConnectionSettings(new Uri(ConnectionString));
            settings.DefaultIndex(DefaultIndex);
            settings.DisableDirectStreaming();
            return new ElasticClient(settings);
        }
    }
}
