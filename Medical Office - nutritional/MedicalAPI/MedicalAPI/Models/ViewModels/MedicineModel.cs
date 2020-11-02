using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class MedicineModel
    {
        public int Duration { get; set; }
        public int DailyDose { get; set; }
        public string Period { get; set; }
        public int IllnessId { get; set; }
        public int MedicamentId { get; set; }
    }
}
