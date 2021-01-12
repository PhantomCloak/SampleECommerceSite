using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.OutputDevices;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFurni.Infrastructure.Repositories
{
    internal class ProductRepository : IProductRepository<ProductFilterParams>
    {
        private EFurniContext _dbContext;
        private IRepositoryQueryOutputDevice _outputDevice;
        public ProductRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(ProductFilterParams filterParams = null, PaginationParams paginationParams = null)
        {
            var query = _dbContext.Product
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x => x.ProductSalesStatistic)
                .Include(x => x.Stock)
                .Include(x => x.CustomerReview)
                .ThenInclude(x => x.Customer)
                .AsQueryable();

            query = AddFilterOnQuery(filterParams, query);
            query = AddSortOnQuery(filterParams, query);

            if (paginationParams == null)
            {
                return await query.ToArrayAsync();
            }
            
            var queryResult = await query.Skip(paginationParams.GetSkipAmount()).Take(paginationParams.PageSize).ToArrayAsync();

            if (_outputDevice != null)
            {
                _outputDevice.SetQueryResultCount(query.Count());

                if (filterParams != null)
                {
                    var totalEntries = await _dbContext.Product.CountAsync();
                    _outputDevice.SetQueryTotalCount(totalEntries);
                }
            }

            return queryResult;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await _dbContext.Product.AddAsync(product);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }


        private static void DisplayStates(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name},State: {entry.State.ToString()} ");
            }
        }


        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Product
                .Include(t => t.Brand)
                .Include(t => t.Category)
                .Include(t => t.ProductSalesStatistic)
                .Include(x => x.CustomerReview)
                .ThenInclude(x => x.Customer)
                .Where(x => x.ProductId == productId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product productToUpdate)
        {
            _dbContext.Product.Update(productToUpdate);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await GetProductByIdAsync(productId);

            if (product == null)
                return false;

            _dbContext.Product.Remove(product);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public void AttachOutputDevice(IRepositoryQueryOutputDevice repositoryQueryOutput)
        {
            _outputDevice = repositoryQueryOutput;
        }

        public IQueryable<Product> AddFilterOnQuery(ProductFilterParams filterParams, IQueryable<Product> query)
        {
            if (filterParams == null)
                return query;

            if (filterParams.ProductIds.Length >= 1)
            {
                query = query.Where(x => filterParams.ProductIds.Any(y => y == x.ProductId));
            }

            if (!string.IsNullOrEmpty(filterParams.CategoryName))
            {
                query = query.Where(f => f.Category.CategoryName == filterParams.CategoryName);
            }

            if (!string.IsNullOrEmpty(filterParams.BrandName))
            {
                query = query.Where(f => f.Brand.BrandName == filterParams.BrandName);
            }

            if (!string.IsNullOrEmpty(filterParams.ProductType))
            {
                query = query.Where(f => f.SubType == filterParams.ProductType);
            }

            if (!string.IsNullOrEmpty(filterParams.ProductColor))
            {
                query = query.Where(f => f.ProductColor == filterParams.ProductColor);
            }

            if (filterParams.MinPrice != null)
            {
                query = query.Where(f => f.ListPrice > filterParams.MinPrice);
            }

            if (filterParams.MaxPrice != null)
            {
                query = query.Where(f => f.ListPrice < filterParams.MaxPrice);
            }

            if (filterParams.MinYear != null)
            {
                query = query.Where(f => f.ModelYear >= filterParams.MinYear);
            }

            if (filterParams.MaxYear != null)
            {
                query = query.Where(f => f.ModelYear <= filterParams.MaxYear);
            }

            return query;
        }

        public IQueryable<Product> AddSortOnQuery(ProductFilterParams filterParams, IQueryable<Product> query)
        {
            if (filterParams == null)
                return query;

            switch (filterParams.Sort)
            {
                case "star":
                    query = query.OrderBy(x => x.ProductSalesStatistic.ProductRating);
                    break;
                case "star_desc":
                    query = query.OrderByDescending(x => x.ProductSalesStatistic.ProductRating);
                    break;
                case "model_year":
                    query = query.OrderBy(x => x.ModelYear);
                    break;
                case "model_year_desc":
                    query = query.OrderByDescending(x => x.ModelYear);
                    break;
                case "list_price":
                    query = query.OrderBy(x => x.ListPrice);
                    break;
                case "list_price_desc":
                    query = query.OrderByDescending(x => x.ListPrice);
                    break;
                case "number_sold":
                    query = query.OrderBy(x => x.ProductSalesStatistic);
                    break;
                case "number_sold_desc":
                    query = query.OrderByDescending(x => x.ProductSalesStatistic);
                    break;
                case "number_viewed":
                    query = query.OrderBy(x => x.ProductSalesStatistic.NumberViewed);
                    break;
                case "number_viewed_desc":
                    query = query.OrderByDescending(x => x.ProductSalesStatistic.NumberViewed);
                    break;
                case "product_type":
                    query = query.OrderBy(x => x.SubType);
                    break;
                case "product_type_desc":
                    query = query.OrderByDescending(x => x.SubType);
                    break;
            }

            return query;
        }
    }
}