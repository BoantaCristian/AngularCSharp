using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccidentAPI.Models
{
    public class ApplicationContext: IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Accident>()
                        .HasOne(m => m.GuiltyPeople)
                        .WithMany(t => t.Guilty)
                        .HasForeignKey(m => m.Guilty)
                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Accident>()
                        .HasOne(m => m.InnocentPeople)
                        .WithMany(t => t.Innocent)
                        .HasForeignKey(m => m.Innocent)
                        .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Accident> Accidents { get; set; }
        public DbSet<People> People { get; set; }
        public DbSet<Severity> Severity { get; set; }
    }
}
