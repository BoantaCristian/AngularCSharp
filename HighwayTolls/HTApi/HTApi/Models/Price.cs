using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTApi.Models
{
    public class Price
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public virtual Location Location { get; set; }
        public virtual Category Category { get; set; }
    }
}
