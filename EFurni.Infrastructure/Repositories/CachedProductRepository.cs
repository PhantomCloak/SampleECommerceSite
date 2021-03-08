using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Extensions;
using EFurni.Infrastructure.OutputDevices;
using EFurni.Shared.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    public class CachedProductRepository : IProductRepository<ProductFilterParams>
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;
        private readonly IProductRepository<ProductFilterParams> _productRepository;

        public CachedProductRepository(
            IProductRepository<ProductFilterParams> productRepository,
            IDistributedCache distributedCache,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _distributedCache = distributedCache;
            _productRepository = productRepository;
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

                    var result = await _distributedCache.GetAsync<IEnumerable<Product>>(cacheKey);

                    if (result == null)
                    {
                        result = await _productRepository.GetAllProductsAsync(null, paginationParams);
                        var cacheOptions = _distributedCache
                            .CacheOptions()
                            .FromConfiguration(_configuration, "ProductCache");
                        
                        await _distributedCache.SetAsync(cacheKey, result,cacheOptions);
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
            var product = await _distributedCache.GetAsync<Product>(cacheKey);

            if (product == null)
            {
                product = await _productRepository.GetProductByIdAsync(productId);
                var cacheOptions = _distributedCache
                    .CacheOptions()
                    .FromConfiguration(_configuration, "ProductCache");
                
                await _distributedCache.SetAsync(cacheKey, product,cacheOptions);
            }

            return product;
        }

        public async Task<bool> UpdateProductAsync(Product productToUpdate)
        {
            var cacheKey = $"product:{productToUpdate.ProductId}";
            await _distributedCache.RemoveAsync(cacheKey);

            return await _productRepository.UpdateProductAsync(productToUpdate);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var cacheKey = $"product:{productId}";
            await _distributedCache.RemoveAsync(cacheKey);

            return await _productRepository.DeleteProductAsync(productId);
        }

        public Task DisposeEntities(IEnumerable<Product> entities)
        {
            return _productRepository.DisposeEntities(entities);
        }

        public void AttachOutputDevice(IRepositoryQueryOutputDevice repositoryQueryOutputDevice) =>
            _productRepository.AttachOutputDevice(repositoryQueryOutputDevice);

        public IQueryable<Product> AddFilterOnQuery(ProductFilterParams filter, IQueryable<Product> query) =>
            _productRepository.AddFilterOnQuery(filter, query);

        public IQueryable<Product> AddSortOnQuery(ProductFilterParams sorter, IQueryable<Product> query) =>
            _productRepository.AddSortOnQuery(sorter, query);
    }
}