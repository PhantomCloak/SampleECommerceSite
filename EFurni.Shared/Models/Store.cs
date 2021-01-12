using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Store
    {
        public Store()
        {
            CustomerOrderItem = new HashSet<CustomerOrderItem>();
            Stock = new HashSet<Stock>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual StoreAddress StoreAddress { get; set; }
        public virtual StoreSalesStatistic StoreSalesStatistic { get; set; }
        public virtual ICollection<CustomerOrderItem> CustomerOrderItem { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
    }
}
