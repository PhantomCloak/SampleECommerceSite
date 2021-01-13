using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    public class CachedCustomerRepository : ICustomerRepository<CustomerFilterParams>
    {
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly IDistributedCacheAdapter _distributedCacheAdapter;
        private readonly TimeSpan _defaultTtl;
        
        public CachedCustomerRepository(
            ICustomerRepository<CustomerFilterParams> customerRepository,
            IConfiguration configuration,
            IDistributedCacheAdapter distributedCacheAdapter)
        {
            _customerRepository = customerRepository;
            _distributedCacheAdapter = distributedCacheAdapter;
            _defaultTtl = TimeSpan.FromMinutes(int.Parse(configuration["CacheProfiles:CustomerTtl"]));
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CustomerFilterParams filterParams,
            PaginationParams paginationParams = null) =>
            await _customerRepository.GetAllCustomersAsync(filterParams, paginationParams);

        public async Task<bool> CreateCustomerAsync(Customer customer) =>
            await _customerRepository.CreateCustomerAsync(customer);

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            var cacheKey = $"customer:{customerId}";
            var customer = await _distributedCacheAdapter.GetAsync<Customer>(cacheKey);

            if (customer == null)
            {
                customer = await _customerRepository.GetCustomerByIdAsync(customerId);
                await _distributedCacheAdapter.SetAsync(cacheKey, customer, _defaultTtl);
            }

            return customer;
        }

        public async Task<Customer> GetCustomerByAccountId(int accountId)
        {
            var cacheKey = $"customer:account:{accountId}";
            var customer = await _distributedCacheAdapter.GetAsync<Customer>(cacheKey);

            if (customer == null)
            {
                customer = await _customerRepository.GetCustomerByAccountId(accountId);
                await _distributedCacheAdapter.SetAsync(cacheKey, customer, _defaultTtl);
            }

            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customerToUpdate)
        {
            var customerCacheKey = $"customer:account:{customerToUpdate.CustomerId}";
            var customerFromAccountCacheKey = $"customer:account:{customerToUpdate.AccountId}";

            await _distributedCacheAdapter.DeleteAsync(customerCacheKey);
            await _distributedCacheAdapter.DeleteAsync(customerFromAccountCacheKey);

            return await _customerRepository.DeleteCustomerAsync(customerToUpdate.CustomerId);
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customerCacheKey = $"customer:account:{customerId}";
            await _distributedCacheAdapter.DeleteAsync(customerCacheKey);
            
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