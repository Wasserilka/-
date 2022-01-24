using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson_1_2.Security
{
    public class AuthResponse
    {
        public int Id { get; set; }

        public RefreshToken RefreshToken { get; set; }

        public string Token { get; set; }
    }
}
