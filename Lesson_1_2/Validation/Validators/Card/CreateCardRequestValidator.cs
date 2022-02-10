using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface ICreateCardRequestValidator : IValidationService<CreateCardRequest> { }

    public class CreateCardRequestValidator : FluentValidationService<CreateCardRequest>, ICreateCardRequestValidator
    {
        public CreateCardRequestValidator()
        {
            RuleFor(x => x.Number.ToString())
                .Length(16)
                .WithMessage("Number length should be 16")
                .WithErrorCode("111.01");

            RuleFor(x => x.HolderName)
                .Length(2, 20)
                .WithMessage("HolderName length should be between 2 and 20")
                .WithErrorCode("111.02");

            RuleFor(x => x.Type)
                .Length(2, 20)
                .WithMessage("Type length should be between 2 and 20")
                .WithErrorCode("111.03");

            RuleFor(x => x.ExpirationDate.ToUnixTimeSeconds())
                .GreaterThan(0)
                .WithMessage("Invalid ExpirationDate")
                .WithErrorCode("111.04");
        }
    }
}
