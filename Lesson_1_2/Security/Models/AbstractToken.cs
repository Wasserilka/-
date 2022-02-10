namespace Lesson_1_2.Security.Models
{
    public abstract class AbstractToken : IToken
    {
        public string Token { get; }

        public AbstractToken(string token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return Token;
        }
    }
}
