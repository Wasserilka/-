using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface ICreateBookRequestValidator : IValidationService<CreateBookRequest> { }

    public class CreateBookRequestValidator : FluentValidationService<CreateBookRequest>, ICreateBookRequestValidator
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .Length(2, 50)
                .WithMessage("Title length should be between 2 and 50")
                .WithErrorCode("121.01");

            RuleFor(x => x.Author)
                .Length(2, 20)
                .WithMessage("Author length should be between 2 and 20")
                .WithErrorCode("121.02");

            RuleFor(x => x.Date.ToUnixTimeSeconds())
                .GreaterThan(0)
                .WithMessage("Invalid ExpirationDate")
                .WithErrorCode("121.03");
        }
    }
}
