using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Patient Patient{ get; set; }
    }
}
