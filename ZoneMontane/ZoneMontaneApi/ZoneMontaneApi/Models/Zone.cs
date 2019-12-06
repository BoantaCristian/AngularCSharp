using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Accommodation> Accommodations { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
        public virtual ICollection<ZoneTeam> ZoneTeams { get; set; }
    }
}
