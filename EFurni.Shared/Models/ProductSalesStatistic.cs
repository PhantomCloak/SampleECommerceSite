using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class ProductSalesStatistic
    {
        public int ProductId { get; set; }
        public int NumberSold { get; set; }
        public double ProductRating { get; set; }
        public int NumberViewed { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual Product Product { get; set; }
    }
}
