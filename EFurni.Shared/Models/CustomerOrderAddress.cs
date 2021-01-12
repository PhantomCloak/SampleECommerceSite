using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class CustomerOrderAddress
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string CountryTag { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string DestinationZipCode { get; set; }
        public string AddressTextPrimary { get; set; }
        public string AddressTextSecondary { get; set; }

        public virtual CustomerOrder Order { get; set; }
    }
}
