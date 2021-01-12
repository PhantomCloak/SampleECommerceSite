#pragma warning disable 8618
using System.Collections.Generic;
using EFurni.Shared.Models;

namespace EFurni.Shared.DTOs
{
    public class CategoryInfoDto : Info
    {
        public IEnumerable<string> CategoryNames { get; set; }
        public IEnumerable<int> TotalProductCounts { get; set; }
    }
}