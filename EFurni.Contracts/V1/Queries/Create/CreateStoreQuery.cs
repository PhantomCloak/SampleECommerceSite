#pragma warning disable 8618
using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateStoreQuery
    {
        [Required]
        [StringLength(32, ErrorMessage = "Store name length can't be less than 1 or more than 32.",MinimumLength = 1)]
        public string StoreName { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(3, ErrorMessage = "Country tag length can't be less than 1 or more than 3.",MinimumLength = 1)]
        public string CountryTag { get; set; }
        
        [Required]
        public string Province { get; set; }
        
        [Required]
        public string District { get; set; }

        [Required]
        public string Neighborhood { get; set; }
        
        [Required]
        public string ZipCode { get; set; }
        
        [Required]
        public string AddressTextPrimary { get; set; }
        
        public string AddressTextSecondary { get; set; }
    }
}