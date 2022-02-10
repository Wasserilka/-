using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IRegisterUserRequestValidator : IValidationService<RegisterUserRequest> { }

    public class RegisterUserRequestValidator : FluentValidationService<RegisterUserRequest>, IRegisterUserRequestValidator
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Login)
                .Length(3, 20)
                .WithMessage("Login length should be between 3 and 20")
                .WithErrorCode("101.01");
        }
    }
}
