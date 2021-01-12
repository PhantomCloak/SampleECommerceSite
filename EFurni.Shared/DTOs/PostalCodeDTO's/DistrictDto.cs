#pragma warning disable 8618
using System.Collections.Generic;

namespace EFurni.Shared.DTOs
{
    public class DistrictDto
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public IEnumerable<NeighborhoodDto> Neighborhoods { get; set; }
    }
}