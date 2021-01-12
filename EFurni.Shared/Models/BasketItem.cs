using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class BasketItem
    {
        public int BasketItemId { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }

        public virtual CustomerBasket Basket { get; set; }
        public virtual Product Product { get; set; }
    }
}
