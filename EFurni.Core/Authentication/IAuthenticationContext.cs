using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EFurni.Core.Authentication
{
    public interface IAuthenticationContext
    {
        void AttachToController(ControllerBase controller);
        string GetCurrentActorIdentifierAsString();
        int GetCurrentActorIdentifier();
    }
}