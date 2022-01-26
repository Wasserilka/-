namespace Lesson_1_2.Security.Models
{
    public class RefreshToken
    { 
        public string Token { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;
    }
}
