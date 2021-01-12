#pragma warning disable 8618
namespace EFurni.Shared.Models
{
    public class GenericAddress
    {
        public string CountryTag { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
    }
}