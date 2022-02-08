using Lesson_1_2.Security.Models;

namespace Lesson_1_2.Security.Responses
{
    public class AuthResponse
    {
        public IToken RefreshToken { get; }
        public IToken MainToken { get; }

        public AuthResponse(IToken refreshToken, IToken mainToken)
        {
            RefreshToken = refreshToken;
            MainToken = mainToken;
        }
    }
}
