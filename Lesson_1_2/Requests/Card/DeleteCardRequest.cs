namespace Lesson_1_2.Requests
{
    public class DeleteCardRequest
    {
        public int Id { get; }

        public DeleteCardRequest(int id)
        {
            Id = id;
        }
    }
}
