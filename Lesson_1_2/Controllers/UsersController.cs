using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Lesson_1_2.Security;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.DAL.Responses;
using Lesson_1_2.DAL.DTO;
using Lesson_1_2.Requests;
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
            var request = new RegisterUserRequest(login, password);

            Repository.Register(request);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string login, string password)
        {
            var request = new AuthenticateUserRequest(login, password);
            var result = Repository.Authenticate(request);

            if (result != 0)
            {
                var authResponse = AuthService.Authenticate(result);
                SetTokenCookie(authResponse.RefreshToken.Token);
                Repository.UpdateToken(authResponse);
                var token = new TokenResponse { Token = authResponse.Token, RefreshToken = authResponse.RefreshToken.Token };
                return Ok(token);
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public IActionResult Refresh()
        {
            string oldRefreshToken = Request.Cookies["refreshToken"];
            var RefreshToken = Repository.GetRefreshToken(oldRefreshToken);
            if (RefreshToken.IsExpired)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            var newRefreshToken = AuthService.RefreshToken(RefreshToken.Id);
            Repository.UpdateToken(RefreshToken.Id, newRefreshToken);
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
