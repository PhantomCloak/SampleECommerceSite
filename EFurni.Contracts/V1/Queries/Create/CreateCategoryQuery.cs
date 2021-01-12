using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateCategoryQuery
    {
        [StringLength(16, ErrorMessage = "Category name length can't be less than 2 or more than 16",MinimumLength = 2)]
        public string CategoryName { get; set; }
    }
}