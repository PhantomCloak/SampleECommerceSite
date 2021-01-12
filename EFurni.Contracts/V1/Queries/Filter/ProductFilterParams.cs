#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.QueryParams
{
    public class ProductFilterParams
    {
        public int[] ProductIds { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        public string? ProductType { get; set; }
        public string? ProductColor { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
        public string? Sort { get; set; }
    }
}
