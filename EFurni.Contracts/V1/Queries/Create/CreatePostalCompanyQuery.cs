using System.ComponentModel.DataAnnotations;

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreatePostalCompanyQuery
    {
        [Required]
        public string PostalCompanyName { get; set; }
        
        [Required]
        public double ServiceFee { get; set; }
    }
}