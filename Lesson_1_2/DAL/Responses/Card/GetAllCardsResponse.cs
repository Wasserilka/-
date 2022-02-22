using Lesson_1_2.DAL.DTO;

namespace Lesson_1_2.DAL.Responses
{
    public class GetAllCardsResponse
    {
        public List<CardDto> Cards { get; }

        public GetAllCardsResponse()
        {
            Cards = new List<CardDto>();
        }
    }
}
