using Lesson_1_2.Security.Models;

namespace Lesson_1_2.Security.Responses
{
    public class TokenResponse
    {
        public List<IToken> tokens { get; }

        public TokenResponse()
        {
            tokens = new List<IToken>();
        }

        public void Add(IToken token)
        {
            tokens.Add(token);
        }
    }
}
