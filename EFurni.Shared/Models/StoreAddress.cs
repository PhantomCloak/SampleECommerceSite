using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class StoreAddress
    {
        public int StoreId { get; set; }
        public string CountryTag { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string AddressTextPrimary { get; set; }
        public string AddressTextSecondary { get; set; }

        public virtual Store Store { get; set; }
    }
}
