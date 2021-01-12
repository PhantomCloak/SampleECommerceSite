#pragma warning disable 8618
using System.ComponentModel.DataAnnotations;
using EFurni.Contract.V1.Queries.QueryParams;

namespace EFurni.Contract.V1.Queries
{
    public class CreateOrderQuery
    {
        [EmailAddress]
        public string OptionalMail { get; set; }
        
        [Required]
        public string DeliveryAddress { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string SecondName { get; set; }
        
        [Required]
        public string PostalServiceName { get; set; }
        
        [Required]
        public string DeliveryZipCode { get; set; }

        public string AdditionalNote { get; set; }
        
        [Required]
        public CreateOrderProductParams[] Products { get; set; }
    }
}