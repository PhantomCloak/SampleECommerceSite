using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.DTOs;
using EFurni.Shared.Models;

namespace EFurni.Presentation.Clients.ClientInterfaces
{
    public interface IReviewClient
    {
        Task<IEnumerable<CustomerReviewDto>> GetReviews(int productId);
        Task<bool> CreateReviewAsync(int productId, string desc,int rating);
    }
}