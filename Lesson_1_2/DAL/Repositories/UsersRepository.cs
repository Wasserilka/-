using Dapper;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.Connection;
using Lesson_1_2.Security;
using Lesson_1_2.Requests;

namespace Lesson_1_2.DAL.Repositories
{
    public interface IUsersRepository
    {
        int Authenticate(AuthenticateUserRequest request);
        void Register(RegisterUserRequest request);
        void UpdateToken(AuthResponse response);
        void UpdateToken(int id, RefreshToken token);
        RefreshToken GetRefreshToken(string oldRefreshmentToken);
    }

    public class UsersRepository : IUsersRepository
    {
        public UsersRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public int Authenticate(AuthenticateUserRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<int>("SELECT id FROM users WHERE login=@login AND password=@password",
                    new { login = request.Login, password = request.Password });
            }
        }

        public void UpdateToken(AuthResponse response)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("UPDATE tokens SET token=@token, expirationdate=@expirationdate WHERE userid=@userid",
                    new { userid = response.Id, token = response.RefreshToken.Token, expirationdate = response.RefreshToken.ExpirationDate.ToUnixTimeSeconds() });
            }
        }

        public void UpdateToken(int id, RefreshToken token)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("UPDATE tokens SET token=@token, expirationdate=@expirationdate WHERE id=@id",
                    new { id =id, token = token.Token, expirationdate = token.ExpirationDate.ToUnixTimeSeconds() });
            }
        }

        public void Register(RegisterUserRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("INSERT INTO users(login, password) VALUES(@login, @password)",
                    new { login = request.Login, password = request.Password });
                var id = connection.QueryFirstOrDefault<int>("SELECT id FROM users WHERE login=@login",
                    new { login = request.Login });
                connection.Execute("INSERT INTO tokens(userid) VALUES(@userid)",
                    new { userid = id });
            }
        }

        public RefreshToken GetRefreshToken(string oldRefreshmentToken)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<RefreshToken>("SELECT id, token, expirationdate FROM tokens WHERE token=@oldtoken",
                    new { oldtoken = oldRefreshmentToken });
            }
        }
    }
}
