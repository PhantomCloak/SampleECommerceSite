using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.DTOs;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IPostalServiceClient
    {
        Task<IEnumerable<PostalServiceDto>> GetPostalServices();
    }
}