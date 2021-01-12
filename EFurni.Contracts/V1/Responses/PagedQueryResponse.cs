
namespace EFurni.Contract.V1.Responses
{
    public class PagedQueryResponse<T> : PagedResponse<T>
    {
        public int QueriedItems { get; set; }
        public int FetchedItems { get; set; }
    }
}