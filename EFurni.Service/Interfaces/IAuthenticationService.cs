using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(string userName,string password);
        Task<bool> RegisterUserAsync(RegisterUserParams registerQuery);
        Task<bool> AuthenticateUser(string token);
        Task<int> GetTokenActorIdAsync(string token);
    }
}