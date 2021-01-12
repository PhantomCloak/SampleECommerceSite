using System.Threading.Tasks;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IAuthenticationClient
    {
        Task<bool> IsLoggedIn();
        Task<string> AuthenticationToken();
        
        Task<bool> Login(string email, string password);
        Task Logout();

        Task<(bool, string)> Register(string username, string password, string firstName, string lastName,string phone);
    }
}