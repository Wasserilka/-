namespace Lesson_1_2.Validation.Service
{
    public interface IOperationResult<TResult>
    {
        IReadOnlyList<IOperationFailure> Failures { get; }

        bool Succeed { get; }
    }

    public class OperationResult<TResult> : IOperationResult<TResult>
    {
        public IReadOnlyList<IOperationFailure> Failures { get; set; }

        public bool Succeed { get; set; }

        public OperationResult(IReadOnlyList<IOperationFailure> failures)
        {
            Failures = failures == null ? new List<IOperationFailure>() : failures;
            Succeed = Failures.Count > 0 ? false : true;
        }
    }
}
