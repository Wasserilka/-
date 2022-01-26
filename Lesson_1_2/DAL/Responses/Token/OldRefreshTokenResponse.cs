namespace Lesson_1_2.DAL.Responses
{
    public class OldRefreshTokenResponse
    {
        public string Login { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;
    }
}
