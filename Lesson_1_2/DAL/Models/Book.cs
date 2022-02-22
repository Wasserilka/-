using MongoDB.Bson;

namespace Lesson_1_2.DAL.Models
{
    public class Book
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
