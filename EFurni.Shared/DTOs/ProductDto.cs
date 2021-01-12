#pragma warning disable 8618
using System;

namespace EFurni.Shared.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SubType { get; set; }
        public string ProductColor { get; set; }
        public float? ProductRating { get; set; }
        public string ProductImage { get; set; }
        public float ProductWidth { get; set; }
        public float ProductHeight { get; set; }
        public float ProductWeight { get; set; }
        public int BoxPieces { get; set; }
        public int ModelYear { get; set; }
        public double ListPrice { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public int NumberViewed { get; set; }
        public int NumberSold { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}