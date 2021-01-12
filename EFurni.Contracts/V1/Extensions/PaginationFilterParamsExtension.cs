using EFurni.Contract.V1.Queries.QueryParams;

namespace EFurni.Contract.V1.Extensions
{
    public static class PaginationFilterParamsExtension
    {
        public static int GetSkipAmount(this PaginationParams instance)
        {
            return (instance.PageNumber - 1) * instance.PageSize;
        }
    }
}