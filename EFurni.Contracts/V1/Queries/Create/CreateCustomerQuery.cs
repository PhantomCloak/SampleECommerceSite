#pragma warning disable 8618
using System.ComponentModel.DataAnnotations;
namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateCustomerQuery
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string ProfilePictureUrl { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }    
    }
}