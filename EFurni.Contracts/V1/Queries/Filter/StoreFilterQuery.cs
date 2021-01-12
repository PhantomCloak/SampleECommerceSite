using System.Collections.Generic;

namespace EFurni.Contract.V1.Queries
{
    public class StoreFilterQuery
    {
        public string? StoreName { get; set; }
        
        public IEnumerable<int>? ProductsInStock { get; set; }
      
        public string? Sort { get; set; }
    }
}