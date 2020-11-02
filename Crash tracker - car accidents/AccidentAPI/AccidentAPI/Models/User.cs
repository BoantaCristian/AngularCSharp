using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccidentAPI.Models
{
    public class User: IdentityUser
    {
        public virtual ICollection<Accident> Agent { get; set; }
        public virtual ICollection<User> Supervisor { get; set; }
        public virtual User UserSupervisor { get; set; }
    }
}
