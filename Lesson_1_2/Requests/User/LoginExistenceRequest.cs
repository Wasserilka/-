namespace Lesson_1_2.Requests
{
    public class LoginExistenceRequest
    {
        public string Login { get; }

        public LoginExistenceRequest(string login)
        {
            Login = login;
        }
    }
}
