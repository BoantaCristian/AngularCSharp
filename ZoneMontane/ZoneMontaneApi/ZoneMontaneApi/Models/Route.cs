using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class Route
    {
        public int Id { get; set; }
        public int Time { get; set; }
        public decimal Length { get; set; }
        public string Mark { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<RouteObjective> RouteObjectives { get; set; }
    }
}
