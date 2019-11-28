using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullApi.Models
{
    public class ApplicationContext: IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Uncle> Uncles { get; set; }
    }
}
