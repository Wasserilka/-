using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Lesson_1_2.Security
{
    public interface ILoginService
    {
        TokenResponse Authenticate(string user, string password);

        string RefreshToken(string token);
    }

    internal sealed class LoginService : ILoginService
    {
        private IDictionary<string, AuthResponse> _users = new Dictionary<string, AuthResponse>()
        {
            {"test", new AuthResponse() { Password = "test"}}
        };

        public const string SecretCode = "some secret_code";

        public TokenResponse Authenticate(string user, string password)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            TokenResponse tokenResponse = new TokenResponse();
            int i = 0;
            foreach (KeyValuePair<string, AuthResponse> pair in _users)
            {
                i++;
                if (string.CompareOrdinal(pair.Key, user) == 0 && string.CompareOrdinal(pair.Value.Password, password) == 0)
                {
                    tokenResponse.Token = GenerateJwtToken(i, 15);
                    RefreshToken refreshToken = GenerateRefreshToken(i, 360);
                    pair.Value.LatestRefreshToken = refreshToken;
                    tokenResponse.RefreshToken = refreshToken.Token;
                    return tokenResponse;
                }
            }
            return null;
        }

        public string RefreshToken(string token)
        {
            int i = 0;
            foreach (KeyValuePair<string, AuthResponse> pair in _users)
            {
                i++;
                if (string.CompareOrdinal(pair.Value.LatestRefreshToken.Token, token) == 0
                    && pair.Value.LatestRefreshToken.IsExpired is false)
                {
                    pair.Value.LatestRefreshToken = GenerateRefreshToken(i, 360);
                    return pair.Value.LatestRefreshToken.Token;
                }
            }
            return string.Empty;
        }

        public RefreshToken GenerateRefreshToken(int id, int minutes)
        {
            RefreshToken refreshToken = new RefreshToken();
            refreshToken.Expires = DateTime.Now.AddMinutes(minutes);
            refreshToken.Token = GenerateJwtToken(id, minutes);
            return refreshToken;
        }

        private string GenerateJwtToken(int id, int minutes)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(SecretCode);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
