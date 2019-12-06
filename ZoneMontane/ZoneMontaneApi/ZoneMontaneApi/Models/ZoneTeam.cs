using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class ZoneTeam
    {
        public int Id { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual Team Team { get; set; }
    }
}
