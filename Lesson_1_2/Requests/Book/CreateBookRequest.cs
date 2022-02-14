namespace Lesson_1_2.Requests
{
    public class CreateBookRequest
    {
        public long Number { get; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTimeOffset Date { get; set; }

        public CreateBookRequest(string title, string author, DateTimeOffset date)
        {
            Title = title;
            Author = author;
            Date = date;
        }
    }
}
