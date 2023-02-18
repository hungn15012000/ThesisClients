using Middleware.Server.Blazor.Services.Base;

namespace Middleware.Server.Blazor.Services.Authentication
{
    public interface IAuthenticationServices
    {
        Task<bool> AuthenticateAsync(LoginUserDto loginModel);
        public Task Logout();
    }
}
