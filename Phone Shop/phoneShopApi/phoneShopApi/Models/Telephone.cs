using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models
{
    public class Telephone
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LaunchDate { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<ShoppingBag> ShoppingBags { get; set; }
        public virtual ICollection<Description> Descriptions { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
