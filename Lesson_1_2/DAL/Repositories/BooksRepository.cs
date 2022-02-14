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
        IList<Card> GetAll();
        Card GetById(GetByIdCardRequest request);
        void Create(CreateCardRequest request);
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

        public IList<Card> GetAll()
        {

        }

        public void Create(CreateCardRequest request)
        {

        }

        public Card GetById(GetByIdCardRequest request)
        {

        }

        public void Update(UpdateCardRequest request)
        {

        }

        public void Delete(DeleteCardRequest request)
        {

        }
    }
}
