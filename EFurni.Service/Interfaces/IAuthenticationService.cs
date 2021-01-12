using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    public interface IAuthenticationService
    {
        Task<(bool Validated,Account ValidatedUser)> ValidateUser(string userName,string password);
        Task<bool> RegisterUserAsync(RegisterUserParams registerQuery);
    }
}