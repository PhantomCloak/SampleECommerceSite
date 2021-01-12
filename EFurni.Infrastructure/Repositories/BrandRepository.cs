using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class BrandRepository : IBrandRepository<BrandFilterParams>
    {
        private readonly EFurniContext _dbContext;
        
        public BrandRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(BrandFilterParams filterParams = null, PaginationParams paginationParams = null)
        {
            var query = _dbContext.Brand
                .Include(x => x.Product)
                .AsQueryable();

            query = AddFilterOnQuery(filterParams, query);
            
            if (paginationParams == null)
            {
                return await query.ToArrayAsync();
            }

            return await query.Skip(paginationParams.GetSkipAmount()).Take(paginationParams.PageSize).ToArrayAsync();
        }

        public async Task<bool> CreateBrandAsync(Brand brand)
        {
            await _dbContext.Brand.AddAsync(brand);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Brand> GetBrandByNameAsync(string brandName)
        {
            return await _dbContext.Brand
                .Include(x => x.Product)
                .Where(x => x.BrandName == brandName)
                .AsQueryable()
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateBrandAsync(Brand brandToUpdate)
        {
            _dbContext.Brand.Update(brandToUpdate);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteBrandAsync(string brandName)
        {
            var post = await GetBrandByNameAsync(brandName);
            
            if (post == null)
                return false;

            _dbContext.Brand.Remove(post);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        
        public IQueryable<Brand> AddFilterOnQuery(BrandFilterParams filterParams, IQueryable<Brand> query)
        {
            if (filterParams == null)
                return query;

            if (filterParams.BrandId != null && filterParams.BrandId != 0)
            {
                query = query.Where(f => f.BrandId == filterParams.BrandId);
            }

            if (filterParams.BrandName != null)
            {
                query = query.Where(f => f.BrandName == filterParams.BrandName);
            }

            return query;
        }

        public IQueryable<Brand> AddSortOnQuery(BrandFilterParams sorter, IQueryable<Brand> query)
        {
            throw new System.NotImplementedException();
        }
    }
}