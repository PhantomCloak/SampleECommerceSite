using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;
using EFurni.Shared.Types;

namespace EFurni.Services
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreDto>> GetAllStoresAsync(StoreFilterParams storeFilterParams = null,PaginationParams paginationParams = null);
        Task<StoreDto> GetStoreAsync(string storeName);
        Task<StoreDto> CreateStoreAsync(CreateStoreParams createStoreParams);
        Task<bool> DeleteStoreAsync(string storeName);
        Task<IEnumerable<StockDto>> GetStocksAsync(int[] products);
    }
}