using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Appointments
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public virtual User User { get; set; }
        public virtual User IdPacient { get; set; }
    }
}
