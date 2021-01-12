using System.Threading.Tasks;
using EFurni.Shared.DTOs;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface ISummaryClient
    {
        Task<ProductInfoDto> GetProductInfoAsync();
        Task<BrandInfoDto> GetBrandInfoAsync();
        Task<CategoryInfoDto> GetCategoryInfoAsync();
    }
}