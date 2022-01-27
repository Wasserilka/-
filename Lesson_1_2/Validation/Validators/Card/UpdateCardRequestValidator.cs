using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IUpdateCardRequestValidator : IValidationService<UpdateCardRequest> { }
    public class UpdateCardRequestValidator : FluentValidationService<UpdateCardRequest>, IUpdateCardRequestValidator
    {
        public UpdateCardRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id should be positive")
                .WithErrorCode("112.05");

            RuleFor(x => x.Number.ToString())
                .Length(16)
                .WithMessage("Number length should be 16")
                .WithErrorCode("112.01");

            RuleFor(x => x.HolderName)
                .Length(2, 20)
                .WithMessage("HolderName length should be between 2 and 20")
                .WithErrorCode("112.02");

            RuleFor(x => x.Type)
                .Length(2, 20)
                .WithMessage("Type length should be between 2 and 20")
                .WithErrorCode("112.03");

            RuleFor(x => x.ExpirationDate.ToUnixTimeSeconds())
                .GreaterThan(0)
                .WithMessage("Invalid ExpirationDate")
                .WithErrorCode("112.04");
        }
    }
}
