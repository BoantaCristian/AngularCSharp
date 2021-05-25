using AssociationAPI.Models.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssociationAPI.Models.ApplicationContext
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ClientProvider>()
                        .HasOne(m => m.WaterProvider)    //field
                        .WithMany(t => t.WaterProvider)  //other table reference
                        .HasForeignKey(m => m.WaterFk)   //key reference
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientProvider>()
                                    .HasOne(m => m.GasProvider)    //field
                                    .WithMany(t => t.GasProvider)  //other table reference
                                    .HasForeignKey(m => m.GasFk)   //key reference
                                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClientProvider>()
                                   .HasOne(m => m.ElectricityProvider)    //field
                                   .WithMany(t => t.ElectricityProvider)  //other table reference
                                   .HasForeignKey(m => m.ElectricityFk)   //key reference
                                   .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<Association> Associations { get; set; }
        public DbSet<ClientProvider> ClientProviders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
