using System.Security.Claims;
using System.Threading.Tasks;
using EFurni.Core.AuthenticationExtension;
using EFurni.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Authentication
{
    public class AuthenticationContext : IAuthenticationContext
    {
        private readonly IAuthenticationService _authenticationService;
        private ControllerBase Controller { get; set; }

        public AuthenticationContext(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void AttachCurrentContext(ControllerBase controller)
        {
            Controller = controller;
        }

        public async Task<int> SenderActorId()
        {
            var token = Controller.User.GetClaim<string>(ClaimTypes.Actor);

            return await _authenticationService.GetTokenActorIdAsync(token);
        }
    }
}