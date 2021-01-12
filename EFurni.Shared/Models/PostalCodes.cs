#pragma warning disable 8618

namespace EFurni.Shared.Models
{
    public class PostalCodes
    {
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string CityIdentifier { get; set; }
        public string District { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? Accuracy { get; set; }
        public string Cordinate { get; set; }
    }
}

