using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class AppointmentModel
    {
        public string MedicId { get; set; }
        public int PatientId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
