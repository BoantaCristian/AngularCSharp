using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public long Telephone { get; set; }
        public int Experience { get; set; }
        public virtual Team Team { get; set; }
    }
}
