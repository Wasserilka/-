using Lesson_1_2.DAL.DTO;

namespace Lesson_1_2.DAL.Responses
{
    public class GetByTitleBookResponse
    {
        public BookDto Book { get; }

        public GetByTitleBookResponse(BookDto book)
        {
            Book = book;
        }
    }
}
