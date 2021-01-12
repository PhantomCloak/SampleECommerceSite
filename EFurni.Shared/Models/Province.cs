using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<Districts>();
        }

        public int ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public string ProvinceName { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<Districts> Districts { get; set; }
    }
}
