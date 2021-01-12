using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFurni.Infrastructure.Repositories
{
    internal class CustomerRepository : ICustomerRepository<CustomerFilterParams>
    {
        private readonly EFurniContext _dbContext;
        
        public CustomerRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CustomerFilterParams filterParams, PaginationParams paginationParams = null)
        {
            var query = _dbContext.Customer
                .Include(x => x.Account)
                .Include(x => x.CustomerOrder)
                .Include(x => x.CustomerBasket)
                .Include(x => x.CustomerBasket)
                .AsQueryable();
            
            query = AddFilterOnQuery(filterParams,query);
            
            if (paginationParams == null)
            {
                return await _dbContext.Customer.ToArrayAsync();
            }

            var data = await query.Skip(paginationParams.GetSkipAmount()).Take(paginationParams.PageSize).ToArrayAsync();
            return data;
        }

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            await _dbContext.Customer.AddAsync(customer);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        
        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _dbContext.Customer
                .Include(x => x.Account)
                .Include(x => x.CustomerOrder)
                .Include(x => x.CustomerBasket)
                .ThenInclude(x => x.BasketItem)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }

        public async Task<Customer> GetCustomerByAccountId(int accountId)
        {
            return await _dbContext.Customer
                .Include(x => x.Account)
                .Include(x => x.CustomerOrder)
                .Include(x => x.CustomerBasket)
                .ThenInclude(x => x.BasketItem)
                .FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<bool> UpdateCustomerAsync(Customer customerToUpdate)
        {
            _dbContext.Customer.Update(customerToUpdate);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            
            if (customer == null)
                return false;

            int result;
            
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                _dbContext.Account.Remove(customer.Account);
                _dbContext.Customer.Remove(customer);

                result = await _dbContext.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
   
            return result > 0;
        }

        public IQueryable<Customer> AddFilterOnQuery(CustomerFilterParams filter, IQueryable<Customer> query)
        {
            if (filter == null)
                return query;

            if (filter.AccountIds != null)
            {
                query = query.Where(f => filter.AccountIds.Contains(f.AccountId));
            }
            
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                query = query.Where(f => f.FirstName == filter.CustomerName);
            }

            if (!string.IsNullOrEmpty(filter.CustomerSurname))
            {
                query = query.Where(f => f.LastName == filter.CustomerSurname);
            }
            
            if (!string.IsNullOrEmpty(filter.PhoneNumber))
            {
                query = query.Where(f => f.PhoneNumber == filter.PhoneNumber);
            }

            return query;
        }

        public IQueryable<Customer> AddSortOnQuery(CustomerFilterParams sorter, IQueryable<Customer> query)
        {
            if (sorter == null)
                return query;
            
            switch (sorter.Sort)
            {
                case "first_name":
                    query = query.OrderBy(x => x.FirstName);
                    break;
                case "first_name_desc":
                    query = query.OrderByDescending(x => x.FirstName);
                    break;
                case "last_name":
                    query = query.OrderBy(x => x.LastName);
                    break;
                case "last_name_desc":
                    query = query.OrderByDescending(x => x.LastName);
                    break;
            }

            return query;
        }
    }
}