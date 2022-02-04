using Lesson_1_2.Security.Models;

namespace Lesson_1_2.Security.Factory
{
    public class MainTokenGenerationFactory : AbstractTokenGenerationFactory, ITokenGenerationFactory
    {
        private string Id { get; }
        private int Minutes { get; }
        public MainTokenGenerationFactory(string secretCode, string id, int minutes) : base(secretCode) 
        {
            Id = id;
            Minutes = minutes;
        }

        public IToken GetToken()
        {
            return new MainToken(GenerateJwtToken(Id, Minutes));
        }
    }
}
