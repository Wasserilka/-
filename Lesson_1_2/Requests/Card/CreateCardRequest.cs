namespace Lesson_1_2.Requests
{
    public class CreateCardRequest
    {
        public long Number { get; }
        public string HolderName { get; }
        public DateTimeOffset ExpirationDate { get; }
        public string Type { get; }

        public CreateCardRequest(long number, string holderName, DateTimeOffset expirationDate, string type)
        {
            Number = number;
            HolderName = holderName;
            ExpirationDate = expirationDate;
            Type = type;
        }
    }
}
