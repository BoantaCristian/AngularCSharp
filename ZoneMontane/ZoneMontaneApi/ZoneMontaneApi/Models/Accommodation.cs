using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public virtual Zone Zone { get; set; }
    }
}
