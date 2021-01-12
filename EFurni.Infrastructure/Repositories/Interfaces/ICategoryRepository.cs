using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface ICategoryRepository  <in TFilter> :
        IQueryFilter<TFilter, Category> where TFilter : class
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(TFilter filterParams = null, PaginationParams paginationParams = null);
        Task<bool> CreateCategoryAsync(Category category);
        Task<Category> GetCategoryByNameAsync(string categoryName);
        Task<bool> UpdateCategoryAsync(Category categoryToUpdate);
        Task<bool> DeleteCategoryAsync(string categoryName);
    }
}