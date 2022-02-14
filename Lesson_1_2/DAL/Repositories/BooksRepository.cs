using Dapper;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.Connection;
using Lesson_1_2.Requests;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lesson_1_2.DAL.Repositories
{
    public interface IBooksRepository
    {
        IList<Book> GetAll();
        Book GetById(GetByIdCardRequest request);
        void Create(CreateBookRequest request);
        void Update(UpdateCardRequest request);
        void Delete(DeleteCardRequest request);
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
            return null;
        }

        public void Create(CreateBookRequest request)
        {
            var connection = new ConnectionManager(Configuration, "MongoDB").GetOpenedConnection("local", "books");
        }

        public Book GetById(GetByIdCardRequest request)
        {
            return null;
        }

        public void Update(UpdateCardRequest request)
        {

        }

        public void Delete(DeleteCardRequest request)
        {

        }
    }
}
