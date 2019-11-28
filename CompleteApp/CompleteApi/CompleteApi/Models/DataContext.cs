using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteApi.Models
{
    public class DataContext: IdentityDbContext
    {
        public DataContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
    }
}
