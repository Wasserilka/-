using System;

namespace Lesson_1_2.Security
{
    public class RefreshToken
    { 
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;
    }
}
