using Dapper;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.Connection;
using Lesson_1_2.Requests;

namespace Lesson_1_2.DAL.Repositories
{
    public interface ICardsRepository
    {
        IList<Card> GetAll();
        Card GetById(GetByIdCardRequest request);
        void Create(CreateCardRequest request);
        void Update(UpdateCardRequest request);
        void Delete(DeleteCardRequest request);
    }

    public class CardsRepository : ICardsRepository
    {
        private IConfiguration Configuration { get; set; }
        public CardsRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public IList<Card> GetAll()
        {
            using (var connection = new PostgreSQLConnectionManager(Configuration).GetOpenedConnection())
            {
                return connection.Query<Card>("SELECT id, number, holdername, expirationdate, type FROM cards").AsList();
            }
        }

        public void Create(CreateCardRequest request)
        {
            using (var connection = new PostgreSQLConnectionManager(Configuration).GetOpenedConnection())
            {
                connection.Query<Card>("INSERT INTO cards(number, holdername, expirationdate, type) VALUES(@number, @holdername, @expirationdate, @type)",
                    new { number = request.Number, holdername = request.HolderName, expirationdate = request.ExpirationDate.ToUnixTimeSeconds(), type = request.Type });
            }
        }

        public Card GetById(GetByIdCardRequest request)
        {
            using (var connection = new PostgreSQLConnectionManager(Configuration).GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<Card>("SELECT * FROM cards WHERE id=@id",
                    new { id = request.Id });
            }
        }

        public void Update(UpdateCardRequest request)
        {
            using (var connection = new PostgreSQLConnectionManager(Configuration).GetOpenedConnection())
            {
                connection.QueryFirstOrDefault<Card>("UPDATE cards SET number=@number, holdername=@holdername, expirationdate=@expirationdate, type=@type WHERE id=@id",
                    new { id = request.Id, number = request.Number, holdername = request.HolderName, expirationdate = request.ExpirationDate.ToUnixTimeSeconds(), type = request.Type });
            }
        }

        public void Delete(DeleteCardRequest request)
        {
            using (var connection = new PostgreSQLConnectionManager(Configuration).GetOpenedConnection())
            {
                connection.QueryFirstOrDefault<Card>("DELETE FROM cards WHERE id=@id",
                    new { id = request.Id });
            }
        }
    }
}
