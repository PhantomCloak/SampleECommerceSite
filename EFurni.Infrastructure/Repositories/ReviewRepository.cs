using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class ReviewRepository : IReviewRepository
    {
        private readonly EFurniContext _dbContext;

        public ReviewRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<CustomerReview>> GetAllCustomerReviewsAsync(int productId)
        {
            return await _dbContext.CustomerReview
                .Include(x=>x.Customer)
                .Include(x=>x.Product)
                .Where(x => x.ProductId == productId)
                .AsQueryable()
                .ToArrayAsync();
        }

        public async Task<CustomerReview> GetCustomerReviewAsync(int reviewId)
        {
            return await _dbContext.CustomerReview
                .Include(x=>x.Customer)
                .Include(x=>x.Product)
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.ReplyReviewId == reviewId);
        }

        public async Task<bool> CreateReviewAsync(CustomerReview customerReview)
        {
            await _dbContext.AddAsync(customerReview);
            await _dbContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> UpdateReviewAsync(CustomerReview customerReview)
        {
            try
            {
                _dbContext.CustomerReview.Update(customerReview);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            try
            {
                _dbContext.CustomerReview.Remove(new CustomerReview(){ReviewId = reviewId});
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}