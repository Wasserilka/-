using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IDeleteCardRequestValidator : IValidationService<DeleteCardRequest> { }

    public class DeleteCardRequestValidator : FluentValidationService<DeleteCardRequest>, IDeleteCardRequestValidator
    {
        public DeleteCardRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id should be positive")
                .WithErrorCode("113.01");
        }
    }
}
