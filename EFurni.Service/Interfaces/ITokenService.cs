using System.Threading.Tasks;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    public interface ITokenService
    {
        public Task<string> GetAccountTokenAsync(Account account);
        public Task<Account> GetTokenAccountAsync(string token);
        public Task<string> CreateTokenAsync(Account account);
        public Task DeleteTokenAsync(string token);
    }
}