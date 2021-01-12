using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class CustomerBasket
    {
        public CustomerBasket()
        {
            BasketItem = new HashSet<BasketItem>();
        }

        public int BasketId { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<BasketItem> BasketItem { get; set; }
    }
}
