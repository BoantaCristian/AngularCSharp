using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Treatment
    {
        [Key]
        public int Id { get; set; }
        public virtual Illness Illness { get; set; }
        public virtual Medicament Medicament { get; set; }
        public int Duration { get; set; }
        public int Quantity { get; set; }
        public int PillPerDay { get; set; }
        public string DayTime { get; set; }
    }
}
