using EFurni.Shared.DTOs;

#pragma warning disable 8618

namespace EFurni.Shared.Models
{
    public class AccessToken
    {
        public string Token{get;set;}
        public AccountDto Account { get; set; }
    }
}
