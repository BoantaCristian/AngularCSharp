using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Telephone { get; set; }
        public string Location { get; set; }
        public virtual ICollection<ZoneTeam> ZoneTeams { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
