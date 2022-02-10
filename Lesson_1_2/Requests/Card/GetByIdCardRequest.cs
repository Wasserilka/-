namespace Lesson_1_2.Requests
{
    public class GetByIdCardRequest
    {
        public int Id { get; }

        public GetByIdCardRequest(int id)
        {
            Id = id;
        }
    }
}
