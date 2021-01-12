using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class BasketRepository : IBasketRepository
    {
        private readonly EFurniContext _dbContext;
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;

        public BasketRepository(EFurniContext dbContext, ICustomerRepository<CustomerFilterParams> customerRepository)
        {
            _dbContext = dbContext;
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string customerIdentifier)
        {
            int customerId = int.Parse(customerIdentifier);
            
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (customer?.CustomerBasket?.BasketItem == null)
            {
                return Enumerable.Empty<BasketItem>();
            }

            var query = _dbContext.BasketItem.Where(x => x.BasketId == customer.CustomerBasket.BasketId)
                .AsQueryable();

            return query.ToArray();
        }

        public async Task<bool> CreateBasketItemAsync(string customerIdentifier, int productId)
        {
            int customerId = int.Parse(customerIdentifier);

            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (customer == null)
            {
                return false;
            }

            if (customer.CustomerBasket == null)
            {
                var newBasket = new CustomerBasket()
                {
                    CustomerId = customer.CustomerId,
                };
                
                await _dbContext.CustomerBasket.AddAsync(newBasket);
                
                bool status = await _dbContext.SaveChangesAsync() > 0;

                if (!status)
                    return false;
                
                customer.CustomerBasket = newBasket;
            }
            
            await _dbContext.BasketItem.AddAsync(new BasketItem()
            {
                BasketId = customer.CustomerBasket.BasketId,
                ProductId = productId,
                Amount = 1
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<BasketItem> GetBasketItemAsync(string customerIdentifier, int productId)
        {
            int customerId = int.Parse(customerIdentifier);

            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (customer?.CustomerBasket?.BasketItem == null)
            {
                return null;
            }

            var basketItem = _dbContext.BasketItem.
                Where(x => x.BasketId == customer.CustomerBasket.BasketId && x.ProductId == productId)
                .AsQueryable().FirstOrDefault();

            return basketItem;
        }

        public async Task<bool> UpdateBasketItemAsync(string customerIdentifier, int productId, int amount)
        {
            int customerId = int.Parse(customerIdentifier);

            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (customer?.CustomerBasket?.BasketItem == null)
            {
                return false;
            }

            var basketItem = await _dbContext.BasketItem.
                Where(x => x.BasketId == customer.CustomerBasket.BasketId && x.ProductId == productId).FirstOrDefaultAsync();

            basketItem.Amount = amount;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBasketItemAsync(string customerIdentifier, int productId)
        {
            int customerId = int.Parse(customerIdentifier);

            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

            if (customer?.CustomerBasket?.BasketItem == null)
            {
                return false;
            }

            var basketItem = await _dbContext.BasketItem
                .Where(x => x.BasketId == customer.CustomerId && x.ProductId == productId).FirstOrDefaultAsync();

            _dbContext.BasketItem.Remove(basketItem);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}