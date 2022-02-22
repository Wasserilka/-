using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IDeleteBookRequestValidator : IValidationService<DeleteBookRequest> { }

    public class DeleteBookRequestValidator : FluentValidationService<DeleteBookRequest>, IDeleteBookRequestValidator
    {
        public DeleteBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .Length(2, 50)
                .WithMessage("Title length should be between 2 and 50")
                .WithErrorCode("123.01");
        }
    }
}
