using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class Objective
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Altitude { get; set; }
        public int Order { get; set; }
        public virtual ICollection<RouteObjective> RouteObjectives { get; set; }
    }
}
