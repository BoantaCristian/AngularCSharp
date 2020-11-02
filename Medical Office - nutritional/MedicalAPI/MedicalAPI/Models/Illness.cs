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
        public string Risk { get; set; }
        public string Symptoms { get; set; }
        public virtual ICollection<Historic> Historics { get; set; }
        public virtual ICollection<Medicine> Medicines { get; set; }
        public virtual ICollection<User> UserIllness { get; set; }
    }
}
