using System.Threading.Tasks;

namespace EFurni.Infrastructure.Repositories
{
    public interface ITokenRepository
    {
        Task<string> CreateTokenAsync(int actorId);
        Task<string> TokenFromActorId(int actorId);
        Task<string> ActorIdFromToken(string token);
        Task<bool> DeleteTokenAsync(string token);
    }
}