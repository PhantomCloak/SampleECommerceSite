using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.OutputDevices;
using EFurni.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    public class CachedProductRepository : IProductRepository<ProductFilterParams>
    {
        private readonly IDistributedCacheAdapter _distributedCacheAdapter;
        private readonly IProductRepository<ProductFilterParams> _productRepository;
        private readonly TimeSpan _defaultTtl;

        public CachedProductRepository(
            IProductRepository<ProductFilterParams> productRepository,
            IDistributedCacheAdapter distributedCacheAdapter,
            IConfiguration configuration)
        {
            _distributedCacheAdapter = distributedCacheAdapter;
            _productRepository = productRepository;
            _defaultTtl = TimeSpan.FromMinutes(int.Parse(configuration["CacheProfiles:ProductTtl"]));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(
            ProductFilterParams filter = null,
            PaginationParams paginationParams = null)
        {
            if (filter == null && paginationParams != null)
            {
                //magic number: most accessed pagination pattern is X page 9 size
                //sake of preventing exploitation this solution works goodish 
                if (paginationParams.PageSize % 9 != 0)
                {
                    var cacheKey = $"productPlainPage:{paginationParams.PageNumber}:{paginationParams.PageSize}";

                    var result = await _distributedCacheAdapter.GetAsync<IEnumerable<Product>>(cacheKey);

                    if (result == null)
                    {
                        result = await _productRepository.GetAllProductsAsync(null, paginationParams);
                        await _distributedCacheAdapter.SetAsync(cacheKey, result,_defaultTtl);
                    }

                    return result;
                }
            }

            return await _productRepository.GetAllProductsAsync(filter, paginationParams);
        }

        public async Task<bool> CreateProductAsync(Product product) =>
            await _productRepository.CreateProductAsync(product);

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var cacheKey = $"product:{productId}";
            var product = await _distributedCacheAdapter.GetAsync<Product>(cacheKey);

            if (product == null)
            {
                product = await _productRepository.GetProductByIdAsync(productId);
                await _distributedCacheAdapter.SetAsync(cacheKey, product, _defaultTtl);
            }

            return product;
        }

        public async Task<bool> UpdateProductAsync(Product productToUpdate)
        {
            var cacheKey = $"product:{productToUpdate.ProductId}";
            await _distributedCacheAdapter.DeleteAsync(cacheKey);

            return await _productRepository.UpdateProductAsync(productToUpdate);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var cacheKey = $"product:{productId}";
            await _distributedCacheAdapter.DeleteAsync(cacheKey);

            return await _productRepository.DeleteProductAsync(productId);
        }

        public void AttachOutputDevice(IRepositoryQueryOutputDevice repositoryQueryOutputDevice) =>
            _productRepository.AttachOutputDevice(repositoryQueryOutputDevice);

        public IQueryable<Product> AddFilterOnQuery(ProductFilterParams filter, IQueryable<Product> query) =>
            _productRepository.AddFilterOnQuery(filter, query);

        public IQueryable<Product> AddSortOnQuery(ProductFilterParams sorter, IQueryable<Product> query) =>
            _productRepository.AddSortOnQuery(sorter, query);
    }
}