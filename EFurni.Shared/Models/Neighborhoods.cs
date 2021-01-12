using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Neighborhoods
    {
        public int NeighborhoodId { get; set; }
        public int? DistrictId { get; set; }
        public string NeighborhoodName { get; set; }
        public string PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public virtual Districts District { get; set; }
    }
}
