#pragma warning disable 8618
using System.Collections.Generic;
using EFurni.Shared.Models;

namespace EFurni.Shared.DTOs
{
    public class ProductInfoDto : Info
    {
        public int TotalProducts { get; set; }
        public double MaxPriceRange { get; set; }
        public double MinPriceRange { get; set; }
        public int MaxYearRange { get; set; }
        public int MinYearRange { get; set; }
        public IEnumerable<string> AvailableColors { get; set; }
    }
}