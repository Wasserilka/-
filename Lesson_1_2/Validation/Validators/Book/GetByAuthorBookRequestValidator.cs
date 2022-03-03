using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IGetByAuthorBookRequestValidator : IValidationService<GetByAuthorBookRequest> { }

    public class GetByAuthorBookRequestValidator : FluentValidationService<GetByAuthorBookRequest>, IGetByAuthorBookRequestValidator
    {
        public GetByAuthorBookRequestValidator()
        {
            RuleFor(x => x.Author)
                .Length(2, 50)
                .WithMessage("Author length should be between 2 and 50")
                .WithErrorCode("125.01");
        }
    }
}
