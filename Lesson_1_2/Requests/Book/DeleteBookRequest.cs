namespace Lesson_1_2.Requests
{
    public class DeleteBookRequest
    {
        public string Title { get; }

        public DeleteBookRequest(string title)
        {
            Title = title;
        }
    }
}
