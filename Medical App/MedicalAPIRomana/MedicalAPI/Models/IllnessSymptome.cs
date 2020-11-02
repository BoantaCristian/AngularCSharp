using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models
{
    public class IllnessSymptome
    {
        [Key]
        public int Id { get; set; }
        public virtual Symptom Symptom { get; set; }
        public virtual Illness Illness { get; set; }
    }
}
