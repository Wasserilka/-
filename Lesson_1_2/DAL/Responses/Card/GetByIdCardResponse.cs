using Lesson_1_2.DAL.DTO;

namespace Lesson_1_2.DAL.Responses
{
    public class GetByIdCardResponse
    {
        public CardDto Card { get; }

        public GetByIdCardResponse(CardDto card)
        {
            Card = card;
        }
    }
}
