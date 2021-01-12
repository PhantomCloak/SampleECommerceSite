#pragma warning disable 8618

namespace EFurni.Shared.DTOs
{
    public class StoreDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        
        public string CountryTag { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public string AddressTextPrimary { get; set; }
        public string AddressTextSecondary { get; set; }
    }
}