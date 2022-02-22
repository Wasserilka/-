namespace Lesson_1_2.Requests
{
    public class GetByAuthorBookRequest
    {
        public string Author { get; }

        public GetByAuthorBookRequest(string author)
        {
            Author = author;
        }
    }
}
