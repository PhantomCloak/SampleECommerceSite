using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
            CustomerReview = new HashSet<CustomerReview>();
        }

        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Account Account { get; set; }
        public virtual CustomerBasket CustomerBasket { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrder { get; set; }
        public virtual ICollection<CustomerReview> CustomerReview { get; set; }
    }
}
