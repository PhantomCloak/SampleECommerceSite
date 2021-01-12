#pragma warning disable 8618
using System.Collections.Generic;

namespace EFurni.Shared.DTOs
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public IEnumerable<ProvinceDto> Provinces { get; set; }
    }
}