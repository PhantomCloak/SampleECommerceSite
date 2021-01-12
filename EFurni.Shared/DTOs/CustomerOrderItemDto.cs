#pragma warning disable 8618

namespace EFurni.Shared.DTOs
{
    public class CustomerOrderItemDto
    {
        public int CustomerOrderItemId { get; set; }
        public int? OrderId { get; set; }
        public string StoreName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double ListPrice { get; set; }
        public int ProductDiscount { get; set; }
    }
}