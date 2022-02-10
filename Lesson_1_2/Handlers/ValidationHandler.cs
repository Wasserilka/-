using Lesson_1_2.Validation.Validators;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Handlers
{
    public class ValidationHandler : AbstractHandler
    {
        private IRegisterUserRequestValidator Validator;
        public ValidationHandler(IRegisterUserRequestValidator validator)
        {
            Validator = validator;
        }
        public override object Handle(object request)
        {
            var validation = new OperationResult<RegisterUserRequest> (Validator.ValidateEntity((RegisterUserRequest)request));

            if (!validation.Succeed)
            {
                return validation;
            }

            return base.Handle(request);
        }
    }
}
