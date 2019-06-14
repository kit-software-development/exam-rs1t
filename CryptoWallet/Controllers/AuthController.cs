using System.Security.Claims;
using System.Threading.Tasks;
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
        public async Task SignIn([FromBody] LoginRequest request)
        {
            var id = await _authService.LoginUser(request);
            await HttpContext.SignInAsync(new ClaimsPrincipal(id));
        }

        [HttpPost("sign-up")]
        public async Task SignUp([FromBody] RegisterRequest request)
        {
            await _authService.RegisterUser(request);
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}