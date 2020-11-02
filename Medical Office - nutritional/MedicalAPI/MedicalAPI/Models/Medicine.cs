using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        public int Duration { get; set; }
        public int DailyDose { get; set; }
        public string Period { get; set; }
        public virtual Illness Illness { get; set; }
        public virtual Medicament Medicament { get; set; }
    }
}
