#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateProductReviewParams
    {
        public int ProductId { get; set; }
        public string ReviewText { get; set; }
        public int AuthorAccountId { get; set; }
        public int Rating { get; set; }
    }
}