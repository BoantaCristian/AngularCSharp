using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class TreatmentModel
    {
        public int IllnessId { get; set; }
        public int MedicamentId { get; set; }
        public int Duration { get; set; }
        public int Quantity { get; set; }
        public int PillPerDay { get; set; }
        public string DayTime { get; set; }
    }
}
