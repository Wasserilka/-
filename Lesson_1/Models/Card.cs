namespace Lesson_1_1.Models
{
    public class Card
    {
        public long Number { get; set; }
        public string HolderName { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public CardType Type { get; set; }

    }
}
