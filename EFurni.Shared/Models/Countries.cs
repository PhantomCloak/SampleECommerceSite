using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Province = new HashSet<Province>();
        }

        public int Id { get; set; }
        public string CountryTag { get; set; }

        public virtual ICollection<Province> Province { get; set; }
    }
}
