using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class CustomerReview
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerComment { get; set; }
        public double CustomerRating { get; set; }
        public int ProductId { get; set; }
        public int? ReplyReviewId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
