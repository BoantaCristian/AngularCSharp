using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HTApi.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TollBooth> TollBooths { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<History> Histories { get; set; }
    }
}
