using System.ComponentModel.DataAnnotations;

#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.QueryParams
{
    public class RegisterUserParams
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}