using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTApi.Models
{
    public class ApplicationContext: IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TollBooth> TollBooths { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<History> Histories { get; set; }
    }
}
