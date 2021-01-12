namespace EFurni.Contract.V1.Queries.QueryParams
{
    public class CustomerFilterParams
    {
        public int[]? AccountIds { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerSurname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Sort { get; set; }
    }
}