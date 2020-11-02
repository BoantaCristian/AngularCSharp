using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models
{
    public class User: IdentityUser
    {
        public virtual ICollection<ShoppingBag> ShoppingBags { get; set; }
        public virtual ICollection<Historic> Historics { get; set; }
    }
}
