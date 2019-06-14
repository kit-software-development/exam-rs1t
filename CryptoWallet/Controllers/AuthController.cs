using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoWallet.Model.Exceptions;
using CryptoWallet.Model.Requests;
using CryptoWallet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWallet.Controllers
{
    public class AuthController : MyControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService service)
        {
            _authService = service;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            try
            {
                var id = await _authService.LoginUser(request);
                await HttpContext.SignInAsync(new ClaimsPrincipal(id));
                return Ok();
            }
            catch (AuthException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.RegisterUser(request);
                return Ok();
            }
            catch (AuthException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}