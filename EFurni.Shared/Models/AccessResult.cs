namespace EFurni.Shared.Models
{
    public class AccessResult<T>
    {
        public T Actor { get; set; }
        public string Token { get; set; }
        public bool Authorized => string.IsNullOrEmpty(Token);
    }
}