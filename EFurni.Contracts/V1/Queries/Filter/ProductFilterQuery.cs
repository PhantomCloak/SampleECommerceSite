using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries
{
    public class ProductFilterQuery
    {
        public int[]? ProductIds { get; set; }

        public string? CategoryName { get; set; }
        
        public string? BrandName { get; set; }
        
        public string? ProductType { get; set; }
        
        public string? ProductColor { get; set; }
        
        [Range(1,10000)]
        public int? MaxPrice { get; set; }

        [Range(1,10000)]
        public int? MinPrice { get; set; }
        
        public int? MinYear { get; set; }
        
        public int? MaxYear { get; set; }
        
        public string? Sort { get; set; }
    }
}