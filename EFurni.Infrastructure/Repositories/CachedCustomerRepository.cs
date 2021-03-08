using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Extensions;
using EFurni.Shared.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    public class CachedCustomerRepository : ICustomerRepository<CustomerFilterParams>
    {
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;
        
        public CachedCustomerRepository(
            ICustomerRepository<CustomerFilterParams> customerRepository,
            IConfiguration configuration,
            IDistributedCache distributedCache)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
            _distributedCache = distributedCache;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CustomerFilterParams filterParams,
            PaginationParams paginationParams = null) =>
            await _customerRepository.GetAllCustomersAsync(filterParams, paginationParams);

        public async Task<bool> CreateCustomerAsync(Customer customer) =>
            await _customerRepository.CreateCustomerAsync(customer);

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            var cacheKey = $"customer:{customerId}";
            var customer = await _distributedCache.GetAsync<Customer>(cacheKey);

            if (customer == null)
            {
                customer = await _customerRepository.GetCustomerByIdAsync(customerId);
                
                var cacheOptions = _distributedCache
                    .CacheOptions()
                    .FromConfiguration(_configuration, "CustomerCache");
                
                await _distributedCache.SetAsync(cacheKey, customer,cacheOptions);
            }

            return customer;
        }

        public async Task<Customer> GetCustomerByAccountId(int accountId)
        {
            var cacheKey = $"customer:account:{accountId}";
            var customer = await _distributedCache.GetAsync<Customer>(cacheKey);

            if (customer == null)
            {
                var cacheOptions = _distributedCache
                    .CacheOptions()
                    .FromConfiguration(_configuration, "CustomerCache");

                customer = await _customerRepository.GetCustomerByAccountId(accountId);
                
                await _distributedCache.SetAsync(cacheKey, customer,cacheOptions);
            }

            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customerToUpdate)
        {
            var customerCacheKey = $"customer:account:{customerToUpdate.CustomerId}";
            var customerFromAccountCacheKey = $"customer:account:{customerToUpdate.AccountId}";

            await _distributedCache.RemoveAsync(customerCacheKey);
            await _distributedCache.RemoveAsync(customerFromAccountCacheKey);

            return await _customerRepository.DeleteCustomerAsync(customerToUpdate.CustomerId);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customerCacheKey = $"customer:account:{customerId}";
            await _distributedCache.RemoveAsync(customerCacheKey);
            
            return await _customerRepository.DeleteCustomerAsync(customerId);
        }

        public IQueryable<Customer> AddFilterOnQuery(CustomerFilterParams filter, IQueryable<Customer> query)
        {
            return _customerRepository.AddFilterOnQuery(filter, query);
        }

        public IQueryable<Customer> AddSortOnQuery(CustomerFilterParams sorter, IQueryable<Customer> query)
        {
            return _customerRepository.AddSortOnQuery(sorter, query);
        }
        
    }
}