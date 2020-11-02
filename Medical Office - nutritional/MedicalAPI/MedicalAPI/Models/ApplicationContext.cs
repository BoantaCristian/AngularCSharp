using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<Historic> Historics { get; set; }
        public DbSet<Illness> Illnesses { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
    }
}
