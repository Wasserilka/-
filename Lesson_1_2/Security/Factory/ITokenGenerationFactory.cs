using Lesson_1_2.Security.Models;

namespace Lesson_1_2.Security.Factory
{
    public interface ITokenGenerationFactory
    {
        IToken GetToken();
    }
}
