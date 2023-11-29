using GearCode.Common.Auth.Forms;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.App.Services
{
    public class AuthState : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            if (IdentityManager.CurrentIdentity != null)
            {
                // Użytkownik jest zalogowany
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, IdentityManager.CurrentIdentity.DisplayName)
                // Możesz dodać więcej informacji z CurrentIdentity jako dodatkowe Claims
            };
                identity = new ClaimsIdentity(claims, "YourAuthenticationType");
            }

            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public void NotifyUserAuthentication(string username)
        {
            var identity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, username),
        }, "YourAuthenticationType");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
