namespace EFurni.Contract.V1.Queries
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 100;
        }

        public PaginationQuery(int pageNumber,int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}