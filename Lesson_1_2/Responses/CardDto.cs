namespace Lesson_1_2.Responses
{
    public class CardDto
    {
        public long Number { get; set; }
        public string HolderName { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public string Type { get; set; }
    }
}
