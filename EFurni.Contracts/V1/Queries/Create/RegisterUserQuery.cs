using System.ComponentModel.DataAnnotations;
#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries
{
    public class RegisterUserQuery
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(16, ErrorMessage = "Name length can't be less than 2 or more than 16.",MinimumLength = 2)]
        public string Password { get; set; }
        
        [Required]
        [StringLength(16, ErrorMessage = "Name length can't be less than 2 or more than 16.",MinimumLength = 2)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(16, ErrorMessage = "Name length can't be less than 2 or more than 16.",MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        
    }
}