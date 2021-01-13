using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.AuthenticationExtension
{
    public interface IAuthenticationContext
    {
        void AttachCurrentContext(ControllerBase controller);
        Task<int> SenderActorId();
    }
}