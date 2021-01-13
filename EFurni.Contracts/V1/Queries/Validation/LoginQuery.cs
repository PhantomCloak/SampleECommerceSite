using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries.Validation
{
    public class LoginQuery
    {
        [Required(ErrorMessage = "The user name or password is incorrect.")]
        public string Username { get; set; }
        
        [Required(ErrorMessage = "The user name or password is incorrect.")]
        public string Password { get; set; }
    }
}