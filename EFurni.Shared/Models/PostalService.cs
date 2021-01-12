using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class PostalService
    {
        public PostalService()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public int ServiceId { get; set; }
        public string Postalservicename { get; set; }
        public double Price { get; set; }
        public int? AvgDeliveryDay { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
