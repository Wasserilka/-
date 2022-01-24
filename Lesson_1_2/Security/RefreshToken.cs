using System;

namespace Lesson_1_2.Security
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}
