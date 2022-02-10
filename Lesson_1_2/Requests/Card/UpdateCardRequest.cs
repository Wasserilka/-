namespace Lesson_1_2.Requests
{
    public class UpdateCardRequest
    {
        public int Id { get; }
        public long Number { get; }
        public string HolderName { get; }
        public DateTimeOffset ExpirationDate { get; }
        public string Type { get; }

        public UpdateCardRequest(int id, long number, string holderName, DateTimeOffset expirationDate, string type)
        {
            Id = id;
            Number = number;
            HolderName = holderName;
            ExpirationDate = expirationDate;
            Type = type;
        }
    }
}
