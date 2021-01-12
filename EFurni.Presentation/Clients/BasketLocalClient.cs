using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EFurni.Presentation.Clients.ClientInterfaces;
using EFurni.Shared.Models;
using Newtonsoft.Json;

namespace EFurni.Presentation.Clients
{
    public class BasketLocalClient : IBasketClient
    {
        private LocalStorageService _localStorageService;

        SemaphoreSlim _semaphorePool = new SemaphoreSlim(10);

        public BasketLocalClient(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        [DebuggerHidden]
        public async Task<IEnumerable<BasketItem>> GetAllBasketProducts()
        {
            try
            {
                await _semaphorePool.WaitAsync();

                Dictionary<int, int> productList = null;

                string productIdsJson = await _localStorageService.GetItemAsStringAsync("productIds");

                if (productIdsJson == null)
                {
                    _semaphorePool.Release();
                    return Enumerable.Empty<BasketItem>();
                }

                productList = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsJson);
                _semaphorePool.Release();

                return productList.Select(x => new BasketItem() {ProductId = x.Key, Amount = x.Value}).ToList();
            }
            catch (Exception e)
            {
                return Enumerable.Empty<BasketItem>();
            }
        }

        public async Task<bool> CreateBasketProductAsync(int productId, int amount)
        {
             await _semaphorePool.WaitAsync();

            Dictionary<int, int> productList = null;

            string productIdsJson = await _localStorageService.GetItemAsStringAsync("productIds");

            if (productIdsJson == null)
            {
                productList = new Dictionary<int, int>();

                productList.Add(productId, amount);
                await _localStorageService.SetItemAsync("productIds", JsonConvert.SerializeObject(productList));

                _semaphorePool.Release();
                return true;
            }

            productList = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsJson);

            if (productList.ContainsKey(productId))
            {
                productList.Remove(productId);
            }


            productList.Add(productId, amount);
            await _localStorageService.SetItemAsync("productIds", JsonConvert.SerializeObject(productList));

            _semaphorePool.Release();

            return true;
        }

        public async Task<BasketItem> GetBasketProduct(int productId)
        {
            await _semaphorePool.WaitAsync();

            Dictionary<int, int> productList = null;

            string productIdsJson = await _localStorageService.GetItemAsStringAsync("productIds");

            if (productIdsJson == null)
            {
                _semaphorePool.Release();
                return null;
            }

            productList = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsJson);

            if (!productList.ContainsKey(productId))
            {
                _semaphorePool.Release();
                return null;
            }

            _semaphorePool.Release();

            return new BasketItem()
            {
                ProductId = productId,
                Amount = productList[productId]
            };
        }

        public async Task<bool> RemoveBasketProduct(int productId)
        {
            await _semaphorePool.WaitAsync();

            Dictionary<int, int> productList = null;

            string productIdsJson = await _localStorageService.GetItemAsStringAsync("productIds");

            if (productIdsJson == null)
            {
                _semaphorePool.Release();
                return false;
            }

            productList = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsJson);

            if (!productList.ContainsKey(productId))
            {
                _semaphorePool.Release();
                return false;
            }

            productList.Remove(productId);
            await _localStorageService.SetItemAsync("productIds", JsonConvert.SerializeObject(productList));

            _semaphorePool.Release();

            return true;
        }
        
        public async Task<bool> UpdateBasketProduct(int productId, int count)
        {
            await _semaphorePool.WaitAsync();

            Dictionary<int, int> productList = null;

            string productIdsJson = await _localStorageService.GetItemAsStringAsync("productIds");

            if (productIdsJson == null)
            {
                _semaphorePool.Release();
                return false;
            }

            productList = JsonConvert.DeserializeObject<Dictionary<int, int>>(productIdsJson);

            if (!productList.ContainsKey(productId))
            {
                _semaphorePool.Release();
                return false;
            }

            productList.Remove(productId);
            productList.Add(productId, count);

            await _localStorageService.SetItemAsync("productIds", JsonConvert.SerializeObject(productList));

            _semaphorePool.Release();

            return true;
        }

        public async Task<bool> UpdateOrAddInc(int productId)
        {
            var basketItem = await GetBasketProduct(productId);

            if (basketItem == null)
            {
                await CreateBasketProductAsync(productId, 1);
            }
            else
            {
                await UpdateBasketProduct(productId, basketItem.Amount + 1);
            }

            return true;
        }

        public async Task ClearAllBasket()
        {
            await _localStorageService.RemoveItemAsync("productIds");
        }
    }
}