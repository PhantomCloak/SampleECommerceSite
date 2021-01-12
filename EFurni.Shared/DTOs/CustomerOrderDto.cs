#pragma warning disable 8618
using System;

namespace EFurni.Shared.DTOs
{
    public class CustomerOrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        
        public string CustomerName { get; set; }
        public string OrderStatus { get; set; }
        
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }

        public string OrderAddress { get; set; }
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string OptionalEmail { get; set; }
        public string CargoCompany { get; set; }
        public double CargoCost { get; set; }
        
        public string CountryTag { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string NeighborhoodName { get; set; }
        public string ShippingPostalCode { get; set; }
        
        public CustomerOrderItemDto[] SubOrders { get; set; }
    }
}