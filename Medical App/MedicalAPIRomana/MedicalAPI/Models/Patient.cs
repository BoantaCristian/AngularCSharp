using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
