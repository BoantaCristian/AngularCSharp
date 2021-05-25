using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.DbModels
{
    public class User : IdentityUser
    {
        public virtual Association Association { get; set; }
        public virtual ICollection<Archive> ClientArchive { get; set; }
        public virtual ICollection<Payment> ClientPayment { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<User> AssociationRepresentative { get; set; }
        public virtual ICollection<ClientProvider> ClientProvider { get; set; }
        public virtual User Representative { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string CNP { get; set; }
    }
}
