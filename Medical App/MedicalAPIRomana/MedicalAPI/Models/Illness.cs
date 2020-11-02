using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class Illness
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Severity { get; set; }
        public virtual ICollection<History> Histories { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
        public virtual ICollection<IllnessSymptome> IllnessSymptomes { get; set; }
    }
}
