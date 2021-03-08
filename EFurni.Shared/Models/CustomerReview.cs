using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFurni.Shared.Models
{
    public partial class CustomerReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
