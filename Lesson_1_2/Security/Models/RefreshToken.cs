namespace Lesson_1_2.Security.Models
{
    public class RefreshToken : AbstractToken, IToken
    {
        public DateTimeOffset ExpirationDate { get; }
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;

        public RefreshToken(string token, DateTimeOffset expirationDate) : base(token)
        {
            ExpirationDate = expirationDate;
        }
    }
}
