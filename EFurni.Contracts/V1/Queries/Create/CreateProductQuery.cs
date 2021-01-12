#pragma warning disable 8618
using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateProductQuery
    {
        [Required]
        [StringLength(256, ErrorMessage = "Product name length can't be less than 6 or more than 256.",MinimumLength = 6)]
        public string ProductName { get; set; }

        [Required]
        public string SubType { get; set; }
        
        [Required]
        public string ProductColor { get; set; }
        
        [Required]
        public string ProductImage { get; set; }
        
        [Required]
        [Range(0.1,float.MaxValue, ErrorMessage = "Product Width should be greater than or equal to 0.1.")]
        public float ProductWidth { get; set; }
        
        [Required]
        [Range(0.1,float.MaxValue, ErrorMessage = "Product Height should be greater than or equal to 0.1.")]
        public float ProductHeight { get; set; }
        
        [Required]
        [Range(0.1,float.MaxValue, ErrorMessage = "Product Weight should be greater than or equal to 0.1.")]
        public float ProductWeight { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Box pieces should be greater than or equal to 1.")]
        public int BoxPieces { get; set; }
       
        [Required]
        public int ModelYear { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "List price should be greater than $1.")]
        public double ListPrice { get; set; }
        
        [Required]
        [StringLength(256, ErrorMessage = "Description length can't be less than 16 or more than 256.",MinimumLength = 16)]
        public string Description { get; set; }
        
        [Required]
        public string BrandName { get; set; }
        
        [Required]
        public string CategoryName { get; set; }
    }
}