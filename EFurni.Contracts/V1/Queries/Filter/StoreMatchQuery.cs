using System.ComponentModel.DataAnnotations;

#pragma warning disable 8618
namespace EFurni.Contract.V1.Queries.Filter
{
    public class StoreMatchQuery
    {
        public int[]? ProductsInStock { get; set; }
        public string? NearestStoreFromZip { get; set; }
    }
}