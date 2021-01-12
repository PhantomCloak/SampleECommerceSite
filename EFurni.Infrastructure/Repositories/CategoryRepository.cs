using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class CategoryRepository : ICategoryRepository<CategoryFilterParams>
    {
        private readonly EFurniContext _dbContext;
        
        public CategoryRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(CategoryFilterParams filterParams = null, PaginationParams paginationParams = null)
        {
            var query = _dbContext.Category
                .Include(x => x.Product)
                .ThenInclude(x=>x.Brand)
                .Include(x=>x.Product)
                .ThenInclude(x=>x.ProductSalesStatistic)
                .AsQueryable();

            if (paginationParams == null)
            {
                return await query.ToArrayAsync();
            }

            query = AddFilterOnQuery(filterParams, query);
            
            return await query.Skip(paginationParams.GetSkipAmount()).Take(paginationParams.PageSize).ToArrayAsync();
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await _dbContext.Category.AddAsync(category);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            return await _dbContext.Category
                .Include(x => x.Product)
                .ThenInclude(x=>x.Brand)
                .Include(x=>x.Product)
                .ThenInclude(x=>x.ProductSalesStatistic)
                .Where(x => x.CategoryName== categoryName)
                .AsQueryable()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateCategoryAsync(Category categoryToUpdate)
        {
            _dbContext.Category.Update(categoryToUpdate);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCategoryAsync(string categoryName)
        {
            var category = await GetCategoryByNameAsync(categoryName);
            
            if (category == null)
                return false;

            _dbContext.Category.Remove(category);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        
        public IQueryable<Category> AddFilterOnQuery(CategoryFilterParams filterParams, IQueryable<Category> query)
        {
            if (filterParams == null)
                return query;

            if (filterParams.CategoryName != null)
            {
                query = query.Where(f => f.CategoryName == filterParams.CategoryName);
            }

            return query;
        }
    }
}