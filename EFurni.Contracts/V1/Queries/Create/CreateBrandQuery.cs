using System.ComponentModel.DataAnnotations;

#pragma warning disable 8618
namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateBrandQuery
    {
        [Required]
        [StringLength(32, ErrorMessage = "Brand name length can't be less than 2 or more than 32.",MinimumLength = 3)]
        public string BrandName { get; set; }
    }
}