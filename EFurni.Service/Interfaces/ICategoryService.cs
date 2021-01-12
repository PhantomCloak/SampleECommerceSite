using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CategoryFilterParams categoryFilterParams = null,PaginationParams paginationParams = null);
        Task<CategoryDto> GetCategoryAsync(string categoryName);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryParams createCategoryParams);
        Task<bool> DeleteCategoryAsync(string categoryName);
    }
}