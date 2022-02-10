using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IGetByIdCardRequestValidator : IValidationService<GetByIdCardRequest> { }

    public class GetByIdCardRequestValidator : FluentValidationService<GetByIdCardRequest>, IGetByIdCardRequestValidator
    {
        public GetByIdCardRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id should be positive")
                .WithErrorCode("114.01");
        }
    }
}
