namespace Lesson_1_2.Requests
{
    public class GetRefreshTokenRequest
    {
        public string OldRefreshToken { get; }

        public GetRefreshTokenRequest(string oldRefreshToken)
        {
            OldRefreshToken = oldRefreshToken;
        }
    }
}
