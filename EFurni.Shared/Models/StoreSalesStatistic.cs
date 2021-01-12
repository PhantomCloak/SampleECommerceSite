using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class StoreSalesStatistic
    {
        public int StoreId { get; set; }
        public int ItemSold { get; set; }

        public virtual Store Store { get; set; }
    }
}
