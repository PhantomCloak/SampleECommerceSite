using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Infrastructure.Extensions;
using EFurni.Shared.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EFurni.Infrastructure.Repositories
{
    internal class AnonymousBasketRepository : IBasketRepository
    {
        private readonly IConnectionMultiplexer _redis;

        public AnonymousBasketRepository(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string customerIdentifier)
        {
            var db = _redis.GetDatabase();

            if (!db.KeyExists(customerIdentifier))
            {
                return null;
            }

            var products = JsonConvert.DeserializeObject<IEnumerable<BasketItem>>(await db.HashGetAsync(customerIdentifier, "productIds"));

            return products;
        }

        public async Task<bool> CreateBasketItemAsync(string customerIdentifier, int productId)
        {
            var productEnum = new List<BasketItem>();

            var db = _redis.GetDatabase();

            var item = await GetAllBasketItemsAsync(customerIdentifier);

            if (item != null)
            {
                var basketItems = item as BasketItem[] ?? item.ToArray();
                if (basketItems.Any())
                {
                    productEnum.AddRange(basketItems);
                }
            }
            
            productEnum.Add(new BasketItem()
            {
                BasketId = customerIdentifier.GetHashMd5(), //this might be cause collision but let's hope it
                ProductId = productId,
                Amount = 1,
            });

            await db.HashSetAsync(customerIdentifier,
                new[]
                {
                    new HashEntry("productIds", JsonConvert.SerializeObject(productEnum))
                });

            return true;
        }

        public async Task<BasketItem> GetBasketItemAsync(string customerIdentifier, int productId)
        {           
            var db = _redis.GetDatabase();

            var productListJson = await db.HashGetAsync(customerIdentifier, "productIds");

            var product = JsonConvert.DeserializeObject<IEnumerable<BasketItem>>(productListJson)
                .FirstOrDefault(x => x.ProductId == productId);

            return product;
        }

        public async Task<bool> UpdateBasketItemAsync(string customerIdentifier, int productId, int amount)
        {
            var db = _redis.GetDatabase();

            var productListJson = await db.HashGetAsync(customerIdentifier, "productIds");
            var productList = JsonConvert.DeserializeObject<IEnumerable<BasketItem>>(productListJson);

            foreach (var basketItem in productList)
            {
                if (basketItem.ProductId == productId)
                {
                    basketItem.Amount = amount;
                }
            }

            await db.HashSetAsync(customerIdentifier, "productIds", JsonConvert.SerializeObject(productList));

            return true;
        }

        public async Task<bool> DeleteBasketItemAsync(string customerIdentifier, int productId)
        {
            var db = _redis.GetDatabase();

            var productJson = db.HashGet(customerIdentifier, "productIds");
            var productList = JsonConvert.DeserializeObject<List<BasketItem>>(productJson);
            var product = productList.First(x => x.ProductId == productId);

            productList.Remove(product);

            if (productList.Count == 0)
            {
                await db.KeyDeleteAsync(customerIdentifier);
            }
            else
            {
                await db.HashSetAsync(customerIdentifier, "productIds", JsonConvert.SerializeObject(productList));
            }

            return true;
        }
    }
}