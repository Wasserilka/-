using Microsoft.AspNetCore.Mvc;
using Lesson_1_2.Security;
using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.Requests;
using Lesson_1_2.Security.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Timesheets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAuthService AuthService;
        private IUsersRepository Repository;
        public UsersController(IAuthService loginService, IUsersRepository userRepository)
        {
            AuthService = loginService;
            Repository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromQuery] string login, string password)
        {
            var loginExistenceRequest = new LoginExistenceRequest(login);
            var loginExistenceResult = Repository.IsLoginExists(loginExistenceRequest);

            if (loginExistenceResult)
            {
                return BadRequest(new { message = "Login already exists" });
            }

            var request = new RegisterUserRequest(login, password);

            Repository.Register(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string login, string password)
        {
            var request = new AuthenticateUserRequest(login, password);
            var authResult = Repository.Authenticate(request);

            if (authResult)
            {
                var authResponse = AuthService.Authenticate(login);
                SetTokenCookie(authResponse.RefreshToken.Token);

                var updateTokenRequest = new UpdateTokenRequest(login, authResponse.RefreshToken);
                Repository.UpdateToken(updateTokenRequest);

                var token = new TokenResponse (authResponse.Token, authResponse.RefreshToken.Token);
                return Ok(token);
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
