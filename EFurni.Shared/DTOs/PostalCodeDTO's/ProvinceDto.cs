#pragma warning disable 8618
using System.Collections.Generic;

namespace EFurni.Shared.DTOs
{
    public class ProvinceDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public IEnumerable<DistrictDto> Districts { get; set; }
    }
}