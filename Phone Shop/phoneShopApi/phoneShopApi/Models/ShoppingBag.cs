using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models
{
    public class ShoppingBag
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public virtual Telephone Telephone { get; set; }
        public virtual User User { get; set; }
    }
}
