using Dapper;
using Lesson_1_2.Models;
using Lesson_1_2.Interfaces;

namespace Lesson_1_2.Repositories
{
    public interface IUserRepository
    {
        void Signin(User user);
        void Signup(User user);
    }

    public class UserRepository : IUserRepository
    {
        public void Signin(User user)
        {
            throw new NotImplementedException();
        }

        public void Signup(User user)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Query<Card>("INSERT INTO users(login, password) VALUES(@login, @password)",
                    new { login = user.Login, password = user.Password });
            }
        }
    }
}
