using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using NewsPortal.Models;
using NewsPortal.Services.Interfaces;
using System.Security.Claims;

namespace NewsPortal.Services.Implementations
{
    public class AuthenticationService : Interfaces.IAuthenticationService
    {
        private readonly IAdminService _adminService;
        private readonly IPasswordHasher<Admin> _passwordHasher;

        public AuthenticationService(IAdminService adminService, IPasswordHasher<Admin> passwordHasher)
        {
            _adminService = adminService;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> SignInAdminAsync(HttpContext httpContext, string email, string password)
        {
            var admin = await _adminService.GetByEmailAsync(email);
            if (admin == null)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
                return false;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "AdminCookie");
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync("AdminCookie", principal);

            return true;
        }

        public async Task SignOutAdminAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync("AdminCookie");
        }
    }
}
