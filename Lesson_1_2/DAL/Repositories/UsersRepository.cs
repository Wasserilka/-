using Dapper;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.Connection;
using Lesson_1_2.Requests;
using Lesson_1_2.DAL.Responses;

namespace Lesson_1_2.DAL.Repositories
{
    public interface IUsersRepository
    {
        bool Authenticate(AuthenticateUserRequest request);
        void Register(RegisterUserRequest request);
        void UpdateToken(UpdateTokenRequest request);
        OldRefreshTokenResponse GetRefreshToken(GetRefreshTokenRequest request);
        bool IsLoginExists(LoginExistenceRequest request);
    }

    public class UsersRepository : IUsersRepository
    {
        public UsersRepository()
        {
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
        }

        public bool Authenticate(AuthenticateUserRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<bool>("SELECT EXISTS (SELECT FROM users WHERE login=@login AND password=@password)",
                    new { login = request.Login, password = request.Password });
            }
        }

        public void UpdateToken(UpdateTokenRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("UPDATE users SET token=@token, expirationdate=@expirationdate WHERE login=@login",
                    new { login = request.Login, token = request.RefreshToken.Token, expirationdate = request.RefreshToken.ExpirationDate.ToUnixTimeSeconds() });
            }
        }

        public void Register(RegisterUserRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("INSERT INTO users(login, password) VALUES(@login, @password)",
                    new { login = request.Login, password = request.Password });
            }
        }

        public OldRefreshTokenResponse GetRefreshToken(GetRefreshTokenRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<OldRefreshTokenResponse>("SELECT login, expirationdate FROM users WHERE token=@oldtoken",
                    new { oldtoken = request.OldRefreshToken });
            }
        }

        public bool IsLoginExists(LoginExistenceRequest request)
        {
            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                return connection.QueryFirstOrDefault<bool>("SELECT EXISTS (SELECT FROM users WHERE login=@login)",
                    new { login = request.Login });
            }
        }
    }
}
