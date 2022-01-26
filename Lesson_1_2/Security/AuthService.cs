using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Lesson_1_2.Security
{
    public interface IAuthService
    {
        AuthResponse Authenticate(int id);
        RefreshToken RefreshToken(int id);
    }

    internal sealed class AuthService : IAuthService
    {
        public const string SecretCode = "some secret_code";

        public AuthResponse Authenticate(int id)
        {
            var authResponse = new AuthResponse();

            authResponse.Id = id;
            authResponse.Token = GenerateJwtToken(id, 15);
            var refreshToken = GenerateRefreshToken(id, 360);
            authResponse.RefreshToken = refreshToken;

            return authResponse;
        }

        public RefreshToken RefreshToken(int id)
        {
            return GenerateRefreshToken(id, 360);
        }

        public RefreshToken GenerateRefreshToken(int id, int minutes)
        {
            RefreshToken refreshToken = new RefreshToken();
            refreshToken.ExpirationDate = DateTime.Now.AddMinutes(minutes);
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
