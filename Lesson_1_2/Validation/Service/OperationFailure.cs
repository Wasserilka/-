namespace Lesson_1_2.Validation.Service
{
    public interface IOperationFailure
    {
        public string PropertyName { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }

    public class OperationFailure : IOperationFailure
    {
        public string PropertyName { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}
