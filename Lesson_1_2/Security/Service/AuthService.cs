using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Lesson_1_2.Security.Models;
using Lesson_1_2.Security.Responses;

namespace Lesson_1_2.Security.Service
{
    public interface IAuthService
    {
        AuthResponse Authenticate(string id);
        RefreshToken RefreshToken(string id);
    }

    internal sealed class AuthService : IAuthService
    {
        public const string SecretCode = "some secret_code";

        public AuthResponse Authenticate(string id)
        {
            var token = GenerateJwtToken(id, 15);
            var refreshToken = GenerateRefreshToken(id, 360);
            var authResponse = new AuthResponse(refreshToken, token);

            return authResponse;
        }

        public RefreshToken RefreshToken(string id)
        {
            return GenerateRefreshToken(id, 360);
        }

        public RefreshToken GenerateRefreshToken(string id, int minutes)
        {
            RefreshToken refreshToken = new RefreshToken();
            refreshToken.ExpirationDate = DateTime.Now.AddMinutes(minutes);
            refreshToken.Token = GenerateJwtToken(id, minutes);
            return refreshToken;
        }

        private string GenerateJwtToken(string id, int minutes)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(SecretCode);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
