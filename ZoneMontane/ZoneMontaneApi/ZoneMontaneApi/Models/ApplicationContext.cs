using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoneMontaneApi.Models
{
    public class ApplicationContext: IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<RouteObjective> RouteObjectives { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<ZoneTeam> ZoneTeams { get; set; }
    }
}
