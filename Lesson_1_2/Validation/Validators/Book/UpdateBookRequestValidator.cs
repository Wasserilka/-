using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IUpdateBookRequestValidator : IValidationService<UpdateBookRequest> { }
    public class UpdateBookRequestValidator : FluentValidationService<UpdateBookRequest>, IUpdateBookRequestValidator
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .Length(2, 50)
                .WithMessage("Title length should be between 2 and 50")
                .WithErrorCode("122.01");

            RuleFor(x => x.Book.Title)
                .Length(2, 50)
                .WithMessage("Title length should be between 2 and 50")
                .WithErrorCode("122.02");

            RuleFor(x => x.Book.Author)
                .Length(2, 20)
                .WithMessage("Author length should be between 2 and 20")
                .WithErrorCode("122.03");

            RuleFor(x => x.Book.Date.ToUnixTimeSeconds())
                .GreaterThan(0)
                .WithMessage("Invalid ExpirationDate")
                .WithErrorCode("122.04");
        }
    }
}
