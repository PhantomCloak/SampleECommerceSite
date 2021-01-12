using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class OrderRepository : IOrderRepository<OrderFilterParams>
    {
        private readonly EFurniContext _dbContext;

        public OrderRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CustomerOrder>> GetAllOrdersAsync(OrderFilterParams filter = null, PaginationParams paginationParams = null)
        {
            var query = _dbContext.CustomerOrder.AsQueryable();

            query = AddFilterOnQuery(filter, query);

            return await query.ToArrayAsync();
        }

        public async Task<CustomerOrder> GetOrderAsync(int orderId)
        {
            var query = _dbContext.CustomerOrder
                .Include(x=>x.PostalService)
                .Include(x=>x.Customer)
                .Include(x=>x.CustomerOrderAddress)
                .Include(x=>x.CustomerOrderItem)
                .ThenInclude(x=>x.Store)
                .Include(x=>x.CustomerOrderItem)
                .ThenInclude(x=>x.Product)
                .AsQueryable();

            return await query.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<bool> CreateOrderAsync(CustomerOrder order)
        {
            // await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbContext.CustomerOrder.AddAsync(order);

                await _dbContext.SaveChangesAsync();
                // await transaction.CommitAsync();
            }
            catch
            {
                // await transaction.RollbackAsync();
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateOrderAsync(CustomerOrder order)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                _dbContext.CustomerOrder.Update(order);
                
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                _dbContext.CustomerOrder.Remove(new CustomerOrder() {OrderId = orderId});
                
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }

            return true;
        }

        public IQueryable<CustomerOrder> AddFilterOnQuery(OrderFilterParams filter, IQueryable<CustomerOrder> query)
        {
            if (filter == null)
                return query;

            if (filter.CustomerId != null)
            {
                query = query.Where(x => x.CustomerId == filter.CustomerId);
            }

            return query;
        }
    }
}