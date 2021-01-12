using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class CustomerOrderItem
    {
        public int CustomerOrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double ListPrice { get; set; }
        public int ProductDiscount { get; set; }
        public int? StoreId { get; set; }

        public virtual CustomerOrder Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
