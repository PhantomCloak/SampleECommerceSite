using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Product = new HashSet<Product>();
        }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
