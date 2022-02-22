using Lesson_1_2.DAL.DTO;

namespace Lesson_1_2.DAL.Responses
{
    public class GetByAuthorBookResponse
    {
        public BookDto Book { get; }

        public GetByAuthorBookResponse(BookDto book)
        {
            Book = book;
        }
    }
}
