using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IStoreRepository<in TFilter> :
        IQueryFilter<TFilter, Store>,
        IQuerySorter<TFilter, Store> where TFilter : class
    {
        Task<IEnumerable<Store>> GetAllStoresAsync(TFilter filter = null, PaginationParams paginationParams = null);
        Task<Store> GetStoreByNameAsync(string storeName);
        Task<bool> CreateStoreAsync(Store store);
        Task<bool> UpdateStoreAsync(Store store);
        Task<bool> DeleteStoreAsync(string storeName);
    }
}