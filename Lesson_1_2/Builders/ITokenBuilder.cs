using Lesson_1_2.Security.Responses;

namespace Lesson_1_2.Builders
{
    public interface ITokenBuilder
    {
        void AddMainToken();
        void AddRefreshToken();
        TokenResponse GetResult();
    }
}
