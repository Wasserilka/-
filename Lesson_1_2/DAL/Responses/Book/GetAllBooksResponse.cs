using Lesson_1_2.DAL.DTO;

namespace Lesson_1_2.DAL.Responses
{
    public class GetAllBooksResponse
    {
        public List<BookDto> Books { get; }

        public GetAllBooksResponse()
        {
            Books = new List<BookDto>();
        }
    }
}
