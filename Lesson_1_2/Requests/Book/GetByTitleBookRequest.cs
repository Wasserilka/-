namespace Lesson_1_2.Requests
{
    public class GetByTitleBookRequest
    {
        public string Title { get; }

        public GetByTitleBookRequest(string title)
        {
            Title = title;
        }
    }
}
