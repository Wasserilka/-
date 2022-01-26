namespace Lesson_1_2.Security
{
    public class TokenResponse
    {
        public string Token { get; }
        public string RefreshToken { get; }

        public TokenResponse(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
