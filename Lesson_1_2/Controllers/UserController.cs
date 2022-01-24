﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Lesson_1_2.Security;
using Lesson_1_2.Models;
using Lesson_1_2.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Timesheets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuthService AuthService;
        private IUserRepository Repository;
        public UserController(IAuthService loginService, IUserRepository userRepository)
        {
            AuthService = loginService;
            Repository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromQuery] string login, string password)
        {
            Repository.Signup(new User { Password = password, Login = login });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromQuery] string login, string password)
        {
            //TokenResponse token = LoginService.Authenticate(user, password);
            //if (token is null)
            //{
            //    return BadRequest(new { message = "Username or password is incorrect" });
            //}
            //SetTokenCookie(token.RefreshToken);
            //return Ok(token);

            var result = Repository.Signin(new User { Password = password, Login = login });
            if (result != null)
            {
                var authResponse = AuthService.Authenticate(result.Id);
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
            string newRefreshToken = AuthService.RefreshToken(oldRefreshToken);

            if (string.IsNullOrWhiteSpace(newRefreshToken))
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            SetTokenCookie(newRefreshToken);
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