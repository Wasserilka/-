using Dapper;
using Lesson_1_2.Models;
using Lesson_1_2.Interfaces;
using Lesson_1_2.Security;

namespace Lesson_1_2.Repositories
{
    public interface IUserRepository
    {
        User Signin(User user);
        void Signup(User user);
        void UpdateToken(AuthResponse response);
    }

    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public User Signin(User user)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                var result = connection.QueryFirstOrDefault<User>("SELECT id, password FROM users WHERE login=@login",
                    new { login = user.Login });
                if (result.Password == user.Password)
                {
                    return result;
                }
            }
            return null;
        }

        public void UpdateToken(AuthResponse response)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("UPDATE tokens SET token=@token, expirationdate=@expirationdate WHERE userid=@userid",
                    new { userid = response.Id, token = response.RefreshToken.Token, expirationdate = response.RefreshToken.Expires.ToUnixTimeSeconds() });
            }
        }

        public void Signup(User user)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("INSERT INTO users(login, password) VALUES(@login, @password)",
                    new { login = user.Login, password = user.Password });
                var id = connection.QueryFirstOrDefault<int>("SELECT id FROM users WHERE login=@login",
                    new { login = user.Login });
                connection.Execute("INSERT INTO tokens(userid) VALUES(@userid)",
                    new { userid = id });
            }
        }
    }
}
