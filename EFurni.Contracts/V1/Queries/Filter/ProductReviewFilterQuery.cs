#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.Filter
{
    public class ProductReviewFilterQuery
    {
        public int? ProductId { get; set; }

        public string? AuthorName { get; set; }
    }
}