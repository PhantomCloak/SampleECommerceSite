#pragma warning disable 8618
using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateProductReviewQuery
    {
        [Required]
        public string ReviewText { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}