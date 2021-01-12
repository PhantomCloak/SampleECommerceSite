#pragma warning disable 8618

namespace EFurni.Shared.DTOs
{
    public class CustomerReviewDto
    {
        public int ReviewId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPicture { get; set; }
        public string CustomerComment { get; set; }
        public float CustomerRating { get; set; }
        public int? ReplyReviewId { get; set; }
    }
}