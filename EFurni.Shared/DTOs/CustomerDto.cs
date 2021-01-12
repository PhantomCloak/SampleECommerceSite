#pragma warning disable 8618

namespace EFurni.Shared.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }
    }
}