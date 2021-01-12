using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Product
    {
        public Product()
        {
            BasketItem = new HashSet<BasketItem>();
            CustomerOrderItem = new HashSet<CustomerOrderItem>();
            CustomerReview = new HashSet<CustomerReview>();
            Stock = new HashSet<Stock>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string SubType { get; set; }
        public string ProductColor { get; set; }
        public string ProductImage { get; set; }
        public double ProductWidth { get; set; }
        public double ProductHeight { get; set; }
        public double ProductWeight { get; set; }
        public int BoxPieces { get; set; }
        public int ModelYear { get; set; }
        public double ListPrice { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public short Listed { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual ProductSalesStatistic ProductSalesStatistic { get; set; }
        public virtual ICollection<BasketItem> BasketItem { get; set; }
        public virtual ICollection<CustomerOrderItem> CustomerOrderItem { get; set; }
        public virtual ICollection<CustomerReview> CustomerReview { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
    }
}
