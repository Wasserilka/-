namespace Lesson_1_2.Security
{
    public class AuthResponse
    {
        public RefreshToken RefreshToken { get; }
        public string Token { get; }

        public AuthResponse(RefreshToken refreshToken, string token)
        {
            RefreshToken = refreshToken;
            Token = token;
        }
    }
}
