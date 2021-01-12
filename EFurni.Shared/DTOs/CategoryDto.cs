
#pragma warning disable 8618
using System.Collections.Generic;

namespace EFurni.Shared.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ProductDto> Products { get; set; }
    }
}