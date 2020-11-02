using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class GetHistoricModel
    {
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string IdMedic { get; set; }
        public string IdPacient { get; set; }
    }
}
