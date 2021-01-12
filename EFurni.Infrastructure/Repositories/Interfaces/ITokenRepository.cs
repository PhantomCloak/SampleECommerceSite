using System.Threading.Tasks;

namespace EFurni.Infrastructure.Repositories
{
    public interface ITokenRepository
    {
        Task<string> CreateTokenAsync(int issuedIdentifier);
        Task<string> GetTokenFromIdentifier(int identifier);
        Task<int> GetIdentifierFromTokenAsync(string token);
        Task<bool> DeleteTokenAsync(string token);
    }
}