using FluentValidation;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Validation.Validators
{
    public interface IGetByTitleBookRequestValidator : IValidationService<GetByTitleBookRequest> { }

    public class GetByTitleBookRequestValidator : FluentValidationService<GetByTitleBookRequest>, IGetByTitleBookRequestValidator
    {
        public GetByTitleBookRequestValidator()
        {
            RuleFor(x => x.Title)
                .Length(2, 50)
                .WithMessage("Title length should be between 2 and 50")
                .WithErrorCode("124.01");
        }
    }
}
