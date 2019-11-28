using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTApi.Models
{
    public class User: IdentityUser
    {
        public virtual ICollection<History> Histories { get; set; }
    }
}
