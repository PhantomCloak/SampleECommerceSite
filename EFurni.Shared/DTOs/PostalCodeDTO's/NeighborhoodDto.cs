#pragma warning disable 8618

namespace EFurni.Shared.DTOs
{
    public class NeighborhoodDto
    {
        public int NeighborhoodId { get; set; }
        public string NeighborhoodName { get; set; }
        public string ZipCode { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}