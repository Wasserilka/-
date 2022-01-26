using Lesson_1_2.Security;

namespace Lesson_1_2.Requests
{
    public class AuthenticateUserRequest
    {
        public string Login { get; }
        public string Password { get; }

        public AuthenticateUserRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
