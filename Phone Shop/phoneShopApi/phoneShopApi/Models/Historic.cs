using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace phoneShopApi.Models
{
    public class Historic
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string Order { get; set; }
        public string PaymentMethod { get; set; }
        public string Contact { get; set; }
        public virtual User User { get; set; }
    }
}
