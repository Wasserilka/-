using System;

namespace Lesson_1_2.Security
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public DateTimeOffset Expires { get; set; }

        public bool IsExpired => DateTimeOffset.UtcNow >= Expires;
    }
}
