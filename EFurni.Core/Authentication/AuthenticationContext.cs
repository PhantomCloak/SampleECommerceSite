using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Authentication
{
    public class AuthenticationContext : IAuthenticationContext
    {
        private ControllerBase _controller { get; set; }
        
        public void AttachToController(ControllerBase controller)
        {
            _controller = controller;
        }

        public string GetCurrentActorIdentifierAsString()
        {
            if (_controller == null)
                return string.Empty;
            
            return _controller.User.GetClaim<string>(ClaimTypes.NameIdentifier);
        }

        public int GetCurrentActorIdentifier()
        {
            if (_controller == null)
                return -1;
            
            return _controller.User.GetClaim<int>(ClaimTypes.NameIdentifier);
        }
    }
}