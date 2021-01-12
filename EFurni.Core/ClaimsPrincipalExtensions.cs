using System;
using System.Linq;
using System.Security.Claims;

namespace EFurni.Core
{
    public static class ClaimsPrincipalExtensions
    {
        public static T GetClaim<T>(this ClaimsPrincipal instance,string claimType)
        {
            var claimStr = instance.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            if (string.IsNullOrEmpty(claimStr))
            {
                return default!;
            }

            var retVal = Convert.ChangeType(claimStr, typeof(T));

            return (T)retVal;
        }
    }
}