using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class User: IdentityUser
    {
        public string Address { get; set; }
        public string IdDoctor { get; set; }
        public virtual ICollection<Details> Details { get; set; }
        public virtual ICollection<Historic> Historics { get; set; }
        [ForeignKey("User")]
        public string Appointments { get; set; }
        [ForeignKey("IdPacient")]
        public string AppointmentsPactient { get; set; }
        public virtual Illness Illnesses { get; set; }
        public bool Cured { get; set; }
    }
}
