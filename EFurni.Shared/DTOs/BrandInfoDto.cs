#pragma warning disable 8618
using System.Collections.Generic;
using EFurni.Shared.Models;

namespace EFurni.Shared.DTOs
{
    public class BrandInfoDto : Info
    {
        public IEnumerable<string> BrandNames { get; set; }
        public IEnumerable<int> TotalProductCounts { get; set; }
    }
}