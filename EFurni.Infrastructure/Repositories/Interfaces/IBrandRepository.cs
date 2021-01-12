using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IBrandRepository<in TFilter> :
        IQueryFilter<TFilter, Brand>,
        IQuerySorter<TFilter, Brand> where TFilter : class
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync(TFilter filterParams = null, PaginationParams paginationParams = null);
        Task<bool> CreateBrandAsync(Brand brand);
        Task<Brand> GetBrandByNameAsync(string brandName);
        Task<bool> UpdateBrandAsync(Brand brandToUpdate);
        Task<bool> DeleteBrandAsync(string brandName);
    }
}