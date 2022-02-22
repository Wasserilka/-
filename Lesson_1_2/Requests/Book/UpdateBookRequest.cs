namespace Lesson_1_2.Requests
{
    public class UpdateBookRequest
    {
        public string Title { get; }
        public UpdateBook Book { get; }

        public UpdateBookRequest(string title, string newTitle, string newAuthor, DateTimeOffset newDate)
        {
            Title = title;
            Book = new UpdateBook(newTitle, newAuthor, newDate);
        }

        public class UpdateBook
        {
            public string Title { get; }
            public string Author { get; }
            public DateTimeOffset Date { get; }

            public UpdateBook(string title, string author, DateTimeOffset date)
            {
                Title = title;
                Author = author;
                Date = date;
            }
        }
    }
}
