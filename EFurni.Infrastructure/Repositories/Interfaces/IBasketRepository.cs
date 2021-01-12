using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IBasketRepository
    {
        Task<IEnumerable<BasketItem>> GetAllBasketItemsAsync(string customerIdentifier);
        Task<bool> CreateBasketItemAsync(string customerIdentifier,int productId);
        Task<BasketItem> GetBasketItemAsync(string customerIdentifier,int productId);
        Task<bool> UpdateBasketItemAsync(string customerIdentifier,int productId,int amount);
        Task<bool> DeleteBasketItemAsync(string customerIdentifier,int productId);
    }
}