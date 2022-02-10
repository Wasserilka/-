using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.Requests;

namespace Lesson_1_2.Handlers
{
    public class LoginExistenceHandler : AbstractHandler
    {
        private IUsersRepository Repository;
        public LoginExistenceHandler(IUsersRepository repository)
        {
            Repository = repository;
        }
        public override object Handle(object request)
        {
            var loginExistenceRequest = new LoginExistenceRequest(((RegisterUserRequest)request).Login);
            var loginExistenceResult = Repository.IsLoginExists(loginExistenceRequest);

            if (loginExistenceResult)
            {
                return new { message = "Login already exists" };
            }

            return base.Handle(request);
        }
    }
}
