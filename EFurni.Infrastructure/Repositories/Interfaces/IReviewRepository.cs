using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<CustomerReview>> GetAllCustomerReviewsAsync(int productId);
        Task<CustomerReview> GetCustomerReviewAsync(int reviewId);
        Task<bool> CreateReviewAsync(CustomerReview customerReview);
        Task<bool> UpdateReviewAsync(CustomerReview customerReview);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}