using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class PostalServiceRepository : IPostalServiceRepository<PostalCompanyFilterParams>
    {
        private readonly EFurniContext _dbContext;

        public PostalServiceRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PostalService>> GetAllPostalServicesAsync(PostalCompanyFilterParams filterParams)
        {
            var query = _dbContext.PostalService.AsQueryable();

            query = AddFilterOnQuery(filterParams,query);
            query = AddSortOnQuery(filterParams,query);

            return query.ToArray();
        }

        public async Task<PostalService> GetPostalServiceByNameAsync(string officeName)
        {
            return await _dbContext.PostalService.AsQueryable().FirstOrDefaultAsync(x => x.Postalservicename == officeName);
        }

        public IQueryable<PostalService> AddFilterOnQuery(PostalCompanyFilterParams filter, IQueryable<PostalService> query)
        {
            return query;
        }

        public IQueryable<PostalService> AddSortOnQuery(PostalCompanyFilterParams sorter, IQueryable<PostalService> query)
        {
            if (sorter == null)
                return query;
            
            switch (sorter.Sort)
            {
                case "by_name":
                    query = query.OrderBy(x => x.Postalservicename);
                    break;
                case "by_name_desc":
                    query = query.OrderByDescending(x => x.Postalservicename);
                    break;
                case "by_price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "by_price_desc":
                    query = query.OrderBy(x => x.Price);
                    break;
            }

            return query;
        }
    }
}