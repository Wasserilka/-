using Lesson_1_2.Security.Models;

namespace Lesson_1_2.Security.Responses
{
    public class AuthResponse
    {
        public IToken RefreshToken { get; }
        public IToken Token { get; }

        public AuthResponse(IToken refreshToken, IToken token)
        {
            RefreshToken = refreshToken;
            Token = token;
        }
    }
}
