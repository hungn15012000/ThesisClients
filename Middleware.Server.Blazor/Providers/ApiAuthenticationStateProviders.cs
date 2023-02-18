using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Middleware.Server.Blazor.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
            jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var notLoggedIn = new ClaimsPrincipal(new ClaimsIdentity());
            var savedToken = await localStorageService.GetItemAsync<string>("accessToken");
            if(savedToken == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            
            var contentToken = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            if(contentToken.ValidTo < DateTime.UtcNow)
            {
                return new AuthenticationState(user);
            }
            var claims = await GetClaims();
            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            return new AuthenticationState(user);
        }

        public async Task LoggedIn ()
        {
            var claims = await GetClaims();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoggedOut()
        {
            await localStorageService.RemoveItemAsync("accessToken");
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authState);
        }
        private async Task<List<Claim>> GetClaims()
        {
            var savedToken = await localStorageService.GetItemAsync<string>("accessToken");
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
    }
}
