using System.Collections.Generic;
#pragma warning disable 8618

namespace EFurni.Contract.V1.Responses
{
    public class PagedResponse <T>
    {
        public IEnumerable<T> Data { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
        
        public PagedResponse(){}

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public PagedResponse(IEnumerable<T> data, int? pageNumber, int? pageSize, string nextPage, string previousPage)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = PageSize;
            PageSize = pageSize;
            NextPage = nextPage;
            PreviousPage = previousPage;
        }
    }
}