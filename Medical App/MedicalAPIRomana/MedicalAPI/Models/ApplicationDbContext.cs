using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Illness> Illnesses { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<IllnessSymptome> IllnessSymptomes { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
