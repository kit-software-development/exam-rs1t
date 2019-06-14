using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoWallet.Database;
using CryptoWallet.Database.Entities;
using CryptoWallet.Model.Exceptions;
using CryptoWallet.Model.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CryptoWallet.Services
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal> LoginUser(LoginRequest request);
        Task RegisterUser(RegisterRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly CryptoWalletDbContext _db;

        public AuthService(CryptoWalletDbContext db)
        {
            _db = db;
        }

        public async Task<ClaimsPrincipal> LoginUser(LoginRequest request)
        {
            const string userNotFoundMessage = "User not found";

            var user = await _db.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user == null)
                throw new AuthException(userNotFoundMessage);

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(null, user.Password, request.Password);

            if (verificationResult != PasswordVerificationResult.Success)
                throw new AuthException(userNotFoundMessage);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return new ClaimsPrincipal(claimsIdentity);
        }

        public async Task RegisterUser(RegisterRequest request)
        {
            var user = await _db.Users.Where(u => u.Email == request.Email).FirstOrDefaultAsync();
            if (user != null)
                throw new AuthException("User already exists");

            var hashedPassword = new PasswordHasher<User>().HashPassword(null, request.Password);
            _db.Users.Add(new User
            {
                Email = request.Email,
                Password = hashedPassword,
                CreatedAt = DateTime.Now,
            });

            await _db.SaveChangesAsync();
        }
    }
}