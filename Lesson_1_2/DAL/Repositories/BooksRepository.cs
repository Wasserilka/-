using Dapper;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.Connection;
using Lesson_1_2.Requests;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace Lesson_1_2.DAL.Repositories
{
    public interface IBooksRepository
    {
        IList<Book> GetAll();
        Book GetByTitle(GetByTitleBookRequest request);
        Book GetByAuthor(GetByAuthorBookRequest request);
        void Create(CreateBookRequest request);
        void Update(UpdateBookRequest request);
        void Delete(DeleteBookRequest request);
    }

    public class BooksRepository : IBooksRepository
    {
        private IConfiguration Configuration { get; set; }
        public BooksRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public IList<Book> GetAll()
        {
            var connection = new MongoDBConnectionManager(Configuration).GetOpenedConnection("local", "books");
            var result = connection.Find(new BsonDocument()).ToList();
            return result.Select(o => BsonSerializer.Deserialize<Book>(o)).ToList();
        }

        public void Create(CreateBookRequest request)
        {
            var connection = new MongoDBConnectionManager(Configuration).GetOpenedConnection("local", "books");
            connection.InsertOne(request.ToBsonDocument());
        }

        public Book GetByTitle(GetByTitleBookRequest request)
        {
            var connection = new ElasticSearchConnectionManager(Configuration).GetOpenedConnection();
            var result = connection.Search<Book>(s => s
            .Query(sq => sq
            .Match(m => m
            .Field(f => f.Title)
            .Query(request.Title))));
            return result.Documents.FirstOrDefault();
        }

        public Book GetByAuthor(GetByAuthorBookRequest request)
        {
            var connection = new ElasticSearchConnectionManager(Configuration).GetOpenedConnection();
            var result = connection.Search<Book>(s => s
            .Query(sq => sq
            .Match(m => m
            .Field(f => f.Title)
            .Query(request.Author))));
            return result.Documents.FirstOrDefault();
        }

        public void Update(UpdateBookRequest request)
        {
            var connection = new MongoDBConnectionManager(Configuration).GetOpenedConnection("local", "books");
            connection.ReplaceOne(new BsonDocument("Title", request.Title), request.Book.ToBsonDocument());
        }

        public void Delete(DeleteBookRequest request)
        {
            var connection = new MongoDBConnectionManager(Configuration).GetOpenedConnection("local", "books");
            connection.DeleteOneAsync(request.ToBsonDocument());
        }
    }
}
