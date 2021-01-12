using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.Filter;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IPostalServiceRepository <in TFilter> :
        IQueryFilter<TFilter, PostalService>,
        IQuerySorter<TFilter, PostalService> where TFilter : class
    {
        Task<IEnumerable<PostalService>> GetAllPostalServicesAsync(PostalCompanyFilterParams filterParams);
        Task<PostalService> GetPostalServiceByNameAsync(string officeName);
    }
}