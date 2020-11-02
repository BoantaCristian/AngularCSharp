using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class PatientModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string MedicId { get; set; }
        public int IllnessId { get; set; }
    }
}
