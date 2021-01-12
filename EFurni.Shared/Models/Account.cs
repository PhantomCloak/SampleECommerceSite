using System;
using System.Collections.Generic;

namespace EFurni.Shared.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
