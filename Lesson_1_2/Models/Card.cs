namespace Lesson_1_2.Models
{
    public class Card
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string HolderName { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public string Type { get; set; }

    }
}
