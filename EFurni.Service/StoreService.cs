
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class StoreService : IStoreService
    {
        private readonly IStoreRepository<StoreFilterParams> _storeRepository;
        private readonly IMapper _mapper;

        public StoreService(IStoreRepository<StoreFilterParams> storeRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StoreDto>> GetAllStoresAsync(StoreFilterParams storeFilterParams = null, PaginationParams paginationParams = null)
        {
            var stores = await _storeRepository.GetAllStoresAsync(storeFilterParams, paginationParams);

            return _mapper.Map<IEnumerable<StoreDto>>(stores);
        }

        public async Task<StoreDto> GetStoreAsync(string storeName)
        {
            var store = await _storeRepository.GetStoreByNameAsync(storeName);

            return _mapper.Map<StoreDto>(store);
        }

        public async Task<StoreDto> CreateStoreAsync(CreateStoreParams createStoreParams)
        {
            var store = new Store
            {
                StoreName = createStoreParams.StoreName,
                PhoneNumber = createStoreParams.PhoneNumber,
                Email = createStoreParams.Email,
                StoreAddress = new StoreAddress
                {
                    CountryTag = createStoreParams.CountryTag,
                    Province = createStoreParams.Province,
                    District = createStoreParams.District,
                    Neighborhood = createStoreParams.Neighborhood,
                    AddressTextPrimary = createStoreParams.AddressTextPrimary,
                    AddressTextSecondary = createStoreParams.AddressTextSecondary
                },
                StoreSalesStatistic = new StoreSalesStatistic()
                {
                    ItemSold = 0
                }
            };

            await _storeRepository.CreateStoreAsync(store);

            return _mapper.Map<StoreDto>(store);
        }

        public async Task<bool> DeleteStoreAsync(string storeName)
        {
            return await _storeRepository.DeleteStoreAsync(storeName);
        }

        public async Task<IEnumerable<StockDto>> GetStocksAsync(int[] products)
        {
            List<Store> stock = (await _storeRepository.GetAllStoresAsync(new StoreFilterParams()
            {
                ProductsInStock = products
            })).ToList();

            IEnumerable<Stock> ss = stock.SelectMany(x=>x.Stock);

            return _mapper.Map<IEnumerable<StockDto>>(ss);
        }
    }
}