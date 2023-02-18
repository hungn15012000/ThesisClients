using Blazored.LocalStorage;
using Middleware.Server.Blazor.Providers;
using Middleware.Server.Blazor.Services.Base;

namespace Middleware.Server.Blazor.Services.Authentication
{
    public class AuthenticationService : IAuthenticationServices
    {
        private readonly IClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly ApiAuthenticationStateProvider authenticationStateProvider;
        public AuthenticationService(IClient httpClient, ILocalStorageService localStorageService, ApiAuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
        {
            var response = await httpClient.LoginAsync(loginModel);
            //Store Token
            await localStorageService.SetItemAsync("accessToken", response.Token);
            //Change state authentication
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();
            return true; 

        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        }
    }
}
