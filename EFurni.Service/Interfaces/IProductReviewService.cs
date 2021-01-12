using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Create;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.DTOs;

namespace EFurni.Services
{
    public interface IProductReviewService
    {
        Task<IEnumerable<CustomerReviewDto>> GetReviewsAsync(int productId);
        Task<CustomerReviewDto> CreateReviewAsync(CreateProductReviewParams createReviewParams);
        Task<bool> UpdateReview(CustomerReviewDto reviewDto);
        Task<bool> DeleteReview(int productId);
    }
}