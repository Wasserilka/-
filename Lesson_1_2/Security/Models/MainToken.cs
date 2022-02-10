namespace Lesson_1_2.Security.Models
{
    public class MainToken : AbstractToken, IToken
    {
        public MainToken(string token) : base(token)
        {
        }
    }
}
