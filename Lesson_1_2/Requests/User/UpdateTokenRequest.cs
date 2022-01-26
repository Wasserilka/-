using Lesson_1_2.Security.Models;

namespace Lesson_1_2.Requests
{
    public class UpdateTokenRequest
    {
        public string Login { get; }
        public RefreshToken RefreshToken { get; }

        public UpdateTokenRequest(string login, RefreshToken refreshToken)
        {
            Login = login;
            RefreshToken = refreshToken;
        }
    }
}
