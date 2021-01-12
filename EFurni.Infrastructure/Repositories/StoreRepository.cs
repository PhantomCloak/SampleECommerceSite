using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class StoreRepository : IStoreRepository<StoreFilterParams>
    {
        private readonly EFurniContext _dbContext;

        public StoreRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync(StoreFilterParams filter = null,
            PaginationParams paginationParams = null)
        {
            var query = _dbContext.Store
                .Include(x => x.Stock)
                .Include(x => x.CustomerOrderItem)
                .Include(x => x.StoreSalesStatistic)
                .Include(x => x.StoreAddress)
                .AsQueryable()
                .AsQueryable();

            query = AddFilterOnQuery(filter, query);
            query = AddSortOnQuery(filter, query);

            IEnumerable<Store> result;

            if (paginationParams != null)
            {
                result = await query
                    .Skip(paginationParams.GetSkipAmount())
                    .Take(paginationParams.PageSize).ToArrayAsync();
            }
            else
            {
                result = await query.ToArrayAsync();
            }

            return result;
        }

        public async Task<Store> GetStoreByNameAsync(string storeName)
        {
            var query = await _dbContext.Store
                .Include(x => x.Stock)
                .Include(x => x.CustomerOrderItem)
                .Include(x => x.StoreSalesStatistic)
                .Include(x => x.StoreAddress)
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.StoreName == storeName);

            return query;
        }

        public async Task<bool> CreateStoreAsync(Store store)
        {
            await _dbContext.Store.AddAsync(store);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                _dbContext.Store.Update(store);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteStoreAsync(string storeName)
        {
            var store = await GetStoreByNameAsync(storeName);

            if (store == null)
                return false;

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                _dbContext.Store.Remove(store);
                await _dbContext.SaveChangesAsync();            
                
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }

            return true;
        }

        public IQueryable<Store> AddFilterOnQuery(StoreFilterParams filter, IQueryable<Store> query)
        {
            if (filter == null)
                return query;

            if (!string.IsNullOrEmpty(filter.StoreName))
            {
                query = query.Where(x => x.StoreName == filter.StoreName);
            }

            if (filter.ProductsInStock != null && filter.ProductsInStock.Any())
            {
                query = query.Where(x => x.Stock.Any(y => filter.ProductsInStock.Contains(y.ProductId)));
            }

            return query;
        }

        public IQueryable<Store> AddSortOnQuery(StoreFilterParams sorter, IQueryable<Store> query)
        {
            if (sorter == null)
                return query;

            switch (sorter.Sort)
            {
                case "quantity_of_product":
                    query = query.OrderBy(x => x.Stock.Select(y => y.Quantity));
                    break;
                case "quantity_of_product_desc":
                    query = query.OrderByDescending(x => x.Stock.Select(y => y.Quantity));
                    break;
                case "number_of_product":
                    query = query.OrderBy(x => x.Stock.Count);
                    break;
                case "number_of_product_desc":
                    query = query.OrderByDescending(x => x.Stock.Count);
                    break;
            }

            return query;
        }
    }
}