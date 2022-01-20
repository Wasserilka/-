using Dapper;
using Lesson_1_2.Models;
using Lesson_1_2.Interfaces;

namespace Lesson_1_2.Repositories
{
    public interface ICardsRepository
    {
        IList<Card> GetAll();
        Card GetById(int id);
        void Create(Card card);
        void Update(int id, Card card);
        void Delete(int id);
    }

    public class CardsRepository : ICardsRepository
    {

        public CardsRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public IList<Card> GetAll()
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.Query<Card>("SELECT id, number, holdername, expirationdate, type FROM cards").AsList();
            }
        }

        public void Create(Card card)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Query<Card>("INSERT INTO cards(number, holdername, expirationdate, type) VALUES(@number, @holdername, @expirationdate, @type)",
                    new { number = card.Number, holdername = card.HolderName, expirationdate = card.ExpirationDate.ToUnixTimeSeconds(), type = card.Type });
            }
        }

        public Card GetById(int id)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<Card>("SELECT * FROM cards WHERE id=@id",
                    new { id = id });
            }
        }

        public void Update(int id, Card card)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.QueryFirstOrDefault<Card>("UPDATE cards SET number=@number, holdername=@holdername, expirationdate=@expirationdate, type=@type WHERE id=@id",
                    new { id = id, number = card.Number, holdername = card.HolderName, expirationdate = card.ExpirationDate.ToUnixTimeSeconds(), type = card.Type });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.QueryFirstOrDefault<Card>("DELETE FROM cards WHERE id=@id",
                    new { id = id });
            }
        }
    }
}
