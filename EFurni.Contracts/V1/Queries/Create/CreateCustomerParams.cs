#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateCustomerParams
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }    
    }
}