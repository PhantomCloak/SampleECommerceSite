#pragma warning disable 8618

namespace EFurni.Contract.V1.Queries.Create
{
    public class CreateProductParams
    {
        public string ProductName { get; set; }
        public string SubType { get; set; }
        public string ProductImage { get; set; }
        public float ProductWidth { get; set; }
        public float ProductHeight { get; set; }
        public float ProductWeight { get; set; }
        public int BoxPieces { get; set; }
        public int ModelYear { get; set; }
        public double ListPrice { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}