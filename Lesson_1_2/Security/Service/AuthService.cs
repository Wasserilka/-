using Lesson_1_2.Security.Models;
using Lesson_1_2.Security.Responses;
using Lesson_1_2.Security.Factory;

namespace Lesson_1_2.Security.Service
{
    public interface IAuthService
    {
        AuthResponse Authenticate(string id);
        IToken RefreshToken(string id);
    }

    internal sealed class AuthService : IAuthService
    {
        public const string SecretCode = "some secret_code";

        public AuthResponse Authenticate(string id)
        {
            return new AuthResponse(
                new RefreshTokenGenerationFactory(SecretCode, id, 360).GetToken(), 
                new MainTokenGenerationFactory(SecretCode, id, 15).GetToken());
        }

        public IToken RefreshToken(string id)
        {
            return new RefreshTokenGenerationFactory(SecretCode, id, 360).GetToken();
        }
    }
}
