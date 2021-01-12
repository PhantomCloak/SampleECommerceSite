#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.QueryParams
{
    public class CreateOrderParams
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? OptionalMail { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryZipCode { get; set; }
        public string PostalServiceName { get; set; }
        public string AdditionalNote { get; set; }
        
        public CreateOrderProductParams[] Products { get; set; }
    }

    public class CreateOrderProductParams
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int Quantity { get; set; }
    }
}