using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class RouteObjective
    {
        public int Id { get; set; }
        public virtual Route Route { get; set; }
        public virtual Objective Objective { get; set; }
    }
}
