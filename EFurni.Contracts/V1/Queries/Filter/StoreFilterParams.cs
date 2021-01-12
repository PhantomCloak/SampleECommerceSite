using System.Collections.Generic;

namespace EFurni.Contract.V1.Queries.QueryParams
{
    public class StoreFilterParams
    {
        public string? StoreName { get; set; }
        public IEnumerable<int>? ProductsInStock { get; set; }
        public string? Sort { get; set; }
        
    }
}