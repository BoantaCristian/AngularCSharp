using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class DetailsModel
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public string BodyType { get; set; }
        public string Activity { get; set; }
    }
}
