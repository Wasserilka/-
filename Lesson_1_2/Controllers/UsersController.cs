using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.Security.Service;
using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.Requests;
using Lesson_1_2.Security.Responses;
using Lesson_1_2.Validation.Validators;
using Lesson_1_2.Validation.Service;
using Lesson_1_2.Handlers;
using Lesson_1_2.Builders;
using Microsoft.AspNetCore.Authorization;

namespace Timesheets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAuthService AuthService;
        private IUsersRepository Repository;
        private IRegisterUserRequestValidator RegisterUserRequestValidator;
        private IAuthenticateUserRequestValidator AuthenticateUserRequestValidator;
        public UsersController(
            IAuthService loginService, 
            IUsersRepository userRepository,
            IRegisterUserRequestValidator registerUserRequestValidator,
            IAuthenticateUserRequestValidator authenticateUserRequestValidator)
        {
            AuthService = loginService;
            Repository = userRepository;
            RegisterUserRequestValidator = registerUserRequestValidator;
            AuthenticateUserRequestValidator = authenticateUserRequestValidator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromQuery] string login, string password)
        {
            var request = new RegisterUserRequest(login, password);

            var validationHandler = new ValidationHandler(RegisterUserRequestValidator);
            var loginExistenceHandler = new LoginExistenceHandler(Repository);

            validationHandler.SetNext(loginExistenceHandler);
            var result = validationHandler.Handle(request);
            if (result != null)
            {
                return BadRequest(result);
            }

            Repository.Register(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string login, string password)
        {
            var request = new AuthenticateUserRequest(login, password);
            var validation = new OperationResult<AuthenticateUserRequest>(AuthenticateUserRequestValidator.ValidateEntity(request));

            if (!validation.Succeed)
            {
                return BadRequest(validation);
            }

            var authResult = Repository.Authenticate(request);

            if (authResult)
            {
                var authResponse = AuthService.Authenticate(login);
                SetTokenCookie(authResponse.RefreshToken.Token);

                var updateTokenRequest = new UpdateTokenRequest(login, authResponse.RefreshToken);
                Repository.UpdateToken(updateTokenRequest);

                var builder = new TokenBuilder(authResponse);
                builder.AddMainToken();
                builder.AddRefreshToken();

                return Ok(builder.GetResult());
            }
            else
            {
                return BadRequest(new { message = "Login or password is incorrect" });
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public IActionResult Refresh()
        {
            var request = new GetRefreshTokenRequest(Request.Cookies["refreshToken"]);
            var oldTokenResponse = Repository.GetRefreshToken(request);

            if (oldTokenResponse.IsExpired)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var newRefreshToken = AuthService.RefreshToken(oldTokenResponse.Login);
            var updateTokenRequest = new UpdateTokenRequest(oldTokenResponse.Login, newRefreshToken);
            Repository.UpdateToken(updateTokenRequest);

            SetTokenCookie(newRefreshToken.Token);

            return Ok(newRefreshToken);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
