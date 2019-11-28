using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FullApi.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual ICollection<Uncle> Uncles { get; set; }
    }
}
