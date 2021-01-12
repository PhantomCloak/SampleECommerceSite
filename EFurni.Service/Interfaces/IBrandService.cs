using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync(BrandFilterParams brandFilterParams = null,PaginationParams paginationParams = null);
        Task<BrandDto> GetBrandAsync(string brandName);
        Task<BrandDto> CreateBrandAsync(CreateBrandParams createBrandParams);
        Task<bool> DeleteBrandAsync(string brandName);
    }
}