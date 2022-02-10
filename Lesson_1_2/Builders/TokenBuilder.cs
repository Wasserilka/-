using Lesson_1_2.Security.Responses;

namespace Lesson_1_2.Builders
{
    public class TokenBuilder : ITokenBuilder
    {
        private TokenResponse TokenResponse;
        private AuthResponse AuthResponse;

        public TokenBuilder(AuthResponse authResponse)
        {
            TokenResponse = new TokenResponse();
            AuthResponse = authResponse;
        }

        public void AddMainToken()
        {
            TokenResponse.Add(AuthResponse.MainToken);
        }

        public void AddRefreshToken()
        {
            TokenResponse.Add(AuthResponse.RefreshToken);
        }

        public TokenResponse GetResult()
        {
            return TokenResponse;
        }
    }
}
