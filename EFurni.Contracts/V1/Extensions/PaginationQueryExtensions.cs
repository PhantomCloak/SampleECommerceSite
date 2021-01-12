using EFurni.Contract.V1.Queries.QueryParams;

namespace EFurni.Contract.V1.Extensions
{
    public static class PaginationQueryExtensions
    {
        public static bool IsValidPagination(this PaginationParams instance)
        {
            return instance.PageSize > 1 && instance.PageNumber > 0;
        }
    }
}