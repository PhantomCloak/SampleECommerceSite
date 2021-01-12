using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.Models;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IBasketClient
    {
        Task<IEnumerable<BasketItem>> GetAllBasketProducts();
        Task<bool> CreateBasketProductAsync(int productId,int amount);
        Task<BasketItem> GetBasketProduct(int productId);
        Task<bool> RemoveBasketProduct(int productId);
        Task<bool> UpdateBasketProduct(int productId,int count);
        Task<bool> UpdateOrAddInc(int productId);
        Task ClearAllBasket();
    }
}