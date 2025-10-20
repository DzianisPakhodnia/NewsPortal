namespace NewsPortal.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAdminAsync(HttpContext httpContext, string email, string password);
        Task SignOutAdminAsync(HttpContext httpContext);
    }

}
