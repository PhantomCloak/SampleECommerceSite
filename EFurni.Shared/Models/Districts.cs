using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Districts
    {
        public Districts()
        {
            Neighborhoods = new HashSet<Neighborhoods>();
        }

        public int DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string DistrictName { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Neighborhoods> Neighborhoods { get; set; }
    }
}
