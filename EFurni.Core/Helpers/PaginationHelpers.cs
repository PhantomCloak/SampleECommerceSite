using System.Collections.Generic;
using System.Linq;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Contract.V1.Responses;
using EFurni.Services;

namespace EFurni.Core.Helpers
{
    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(
            IUriGeneratorService uriGeneratorService,
            PaginationParams pagination,
            IEnumerable<T> response)
        {
            var nextPage = pagination.PageNumber >= 1
                ? uriGeneratorService.GetAllEntitiesUri(new PaginationQuery(pagination.PageNumber + 1, pagination.PageSize))
                    .ToString()
                : null;

            var previousPage = pagination.PageNumber - 1 >= 1
                ? uriGeneratorService.GetAllEntitiesUri(new PaginationQuery(pagination.PageNumber - 1, pagination.PageSize))
                    .ToString()
                : null;

            //TODO similar expression
            var enumerable = response as T[] ?? response.ToArray();

            int? pageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?) null;
            int? pageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?) null;
            
            return new PagedResponse<T>(enumerable,
                pageNumber,
                pageSize,
                nextPage!,
                previousPage!);
        }

        public static PagedQueryResponse<T> CreatePaginatedQueryResponse<T>(
            IUriGeneratorService uriGeneratorService,
            PaginationParams pagination,
            IEnumerable<T> response,
            int count)
        {
            //safe casting
            var enumerable = response.ToList();
            var pagedResponse = CreatePaginatedResponse(uriGeneratorService, pagination, enumerable);
            
            var queryResponse = new PagedQueryResponse<T>
            {
                Data = pagedResponse.Data,
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize,
                QueriedItems = count,
                FetchedItems = enumerable.Count() > pagination.PageSize ? enumerable.Count() : pagination.PageSize
            };
            
            return queryResponse;
        }
    }
}