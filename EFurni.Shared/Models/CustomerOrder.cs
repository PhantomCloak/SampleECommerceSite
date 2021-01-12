using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class CustomerOrder
    {
        public CustomerOrder()
        {
            CustomerOrderItem = new HashSet<CustomerOrderItem>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string ReceiverFirst { get; set; }
        public string ReceiverLast { get; set; }
        public string PhoneNumber { get; set; }
        public string OptionalMail { get; set; }
        public int? PostalServiceId { get; set; }
        public int CouponDiscount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public string AdditionalNote { get; set; }
        public double TotalPrice { get; set; }
        public double CargoPrice { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual PostalService PostalService { get; set; }
        public virtual CustomerOrderAddress CustomerOrderAddress { get; set; }
        public virtual ICollection<CustomerOrderItem> CustomerOrderItem { get; set; }
    }
}
