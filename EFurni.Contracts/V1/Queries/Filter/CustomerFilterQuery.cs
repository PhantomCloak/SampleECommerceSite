#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.Filter
{
    public class CustomerFilterQuery
    {
        public string? CustomerName { get; set; }
        
        public string? CustomerSurname { get; set; }

        public string? PhoneNumber { get; set; }
        
        public string? Sort { get; set; }
    }
}