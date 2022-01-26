namespace Lesson_1_2.Requests
{
    public class RegisterUserRequest
    {
        public string Login { get; }
        public string Password { get; }

        public RegisterUserRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
